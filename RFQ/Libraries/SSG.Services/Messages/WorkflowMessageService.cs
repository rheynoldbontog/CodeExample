﻿using System;
using System.Collections.Generic;
using System.Linq;
using SSG.Core.Domain.Forums;
using SSG.Core.Domain.Messages;
using SSG.Core.Domain.News;
using SSG.Core.Domain.RFQ;
using SSG.Core.Domain.Users;
using SSG.Services.Events;
using SSG.Services.Localization;
using SSG.Services.Users;
using System.Text;
using System.IO;

namespace SSG.Services.Messages
{
    public partial class WorkflowMessageService : IWorkflowMessageService
    {
        #region Fields

        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly ILanguageService _languageService;
        private readonly ITokenizer _tokenizer;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IUserService _userService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public WorkflowMessageService(IMessageTemplateService messageTemplateService,
            IQueuedEmailService queuedEmailService, ILanguageService languageService,
            ITokenizer tokenizer, IEmailAccountService emailAccountService,
            IMessageTokenProvider messageTokenProvider,
            EmailAccountSettings emailAccountSettings,
            IEventPublisher eventPublisher,
            IUserService userService)
        {
            this._messageTemplateService = messageTemplateService;
            this._queuedEmailService = queuedEmailService;
            this._languageService = languageService;
            this._tokenizer = tokenizer;
            this._emailAccountService = emailAccountService;
            this._messageTokenProvider = messageTokenProvider;
            this._userService = userService;
            this._emailAccountSettings = emailAccountSettings;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Utilities
        
        private int SendNotification(MessageTemplate messageTemplate, 
            EmailAccount emailAccount, int languageId, IEnumerable<Token> tokens,
            string toEmailAddress, string toName, string attachments = null)
        {
            //retrieve localized message template data
            var bcc = messageTemplate.GetLocalized((mt) => mt.BccEmailAddresses, languageId);
            var subject = messageTemplate.GetLocalized((mt) => mt.Subject, languageId);
            var body = messageTemplate.GetLocalized((mt) => mt.Body, languageId);
            
            //Replace subject and body tokens 
            var subjectReplaced = _tokenizer.Replace(subject, tokens, false);
            var bodyReplaced = _tokenizer.Replace(body, tokens, true);
            
            var email = new QueuedEmail()
            {
                Priority = 5,
                From = emailAccount.Email,
                FromName = emailAccount.DisplayName,
                To = toEmailAddress,
                ToName = toName,
                CC = string.Empty,
                Bcc = bcc,
                Subject = subjectReplaced,
                Body = bodyReplaced,
                CreatedOnUtc = DateTime.UtcNow,
                EmailAccountId = emailAccount.Id,
                Attachments = attachments
            };

            _queuedEmailService.InsertQueuedEmail(email);
            return email.Id;
        }
        
        private IList<Token> GenerateTokens(User user)
        {
            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddUserTokens(tokens, user);
            return tokens;
        }

        private IList<Token> GenerateTokens(NewsLetterSubscription subscription)
        {
            var tokens = new List<Token>();

            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddNewsLetterSubscriptionTokens(tokens, subscription);
            
            return tokens;
        }

        private IList<Token> GenerateTokens(NewsComment newsComment)
        {
            var tokens = new List<Token>();

            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddNewsCommentTokens(tokens, newsComment);

            return tokens;
        }

        private IList<Token> GenerateTokens(ForumTopic forumTopic)
        {
            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddForumTopicTokens(tokens, forumTopic);
            _messageTokenProvider.AddForumTokens(tokens, forumTopic.Forum);
            return tokens;            
        }

        private IList<Token> GenerateTokens(ForumPost forumPost, int friendlyForumTopicPageIndex,
            int appendPostIdentifier)
        {
            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddForumPostTokens(tokens, forumPost);
            _messageTokenProvider.AddForumTopicTokens(tokens, forumPost.ForumTopic,
                friendlyForumTopicPageIndex, appendPostIdentifier);
            _messageTokenProvider.AddForumTokens(tokens, forumPost.ForumTopic.Forum);

            return tokens;
        }
        
        private IList<Token> GenerateTokens(PrivateMessage privateMessage)
        {
            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddPrivateMessageTokens(tokens, privateMessage);
            return tokens;
        }
        
        private MessageTemplate GetLocalizedActiveMessageTemplate(string messageTemplateName, int languageId)
        {
            var messageTemplate = _messageTemplateService.GetMessageTemplateByName(messageTemplateName);
            if (messageTemplate == null)
                return null;

            //var isActive = messageTemplate.GetLocalized((mt) => mt.IsActive, languageId);
            //use
            var isActive = messageTemplate.IsActive;
            if (!isActive)
                return null;

            return messageTemplate;
        }

        private EmailAccount GetEmailAccountOfMessageTemplate(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccounId = messageTemplate.GetLocalized(mt => mt.EmailAccountId, languageId);
            var emailAccount = _emailAccountService.GetEmailAccountById(emailAccounId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            return emailAccount;

        }

        private int EnsureLanguageIsActive(int languageId)
        {
            var language = _languageService.GetLanguageById(languageId);
            if (language == null || !language.Published)
                language = _languageService.GetAllLanguages().FirstOrDefault();
            return language.Id;
        }

        #endregion

        #region Methods

        #region User workflow

        /// <summary>
        /// Sends 'New user' notification message to a site owner
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendUserRegisteredNotificationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("NewUser.Notification", languageId);
            if (messageTemplate == null)
                return 0;

            var userTokens = GenerateTokens(user);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, userTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;
            return SendNotification(messageTemplate, emailAccount,
                languageId, userTokens,
                toEmail, toName);
        }

        /// <summary>
        /// Sends a welcome message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendUserWelcomeMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("User.WelcomeMessage", languageId);
            if (messageTemplate == null)
                return 0;

            var userTokens = GenerateTokens(user);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, userTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = user.Email;
            var toName = user.GetFullName();
            return SendNotification(messageTemplate, emailAccount,
                languageId, userTokens, 
                toEmail, toName);
        }

        /// <summary>
        /// Sends an email validation message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendUserEmailValidationMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("User.EmailValidationMessage", languageId);
            if (messageTemplate == null)
                return 0;

            var userTokens = GenerateTokens(user);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, userTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = user.Email;
            var toName = user.GetFullName();
            return SendNotification(messageTemplate, emailAccount,
                languageId, userTokens,
                toEmail, toName);
        }

        /// <summary>
        /// Sends password recovery message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendUserPasswordRecoveryMessage(User user, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            
            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("User.PasswordRecovery", languageId);
            if (messageTemplate == null)
                return 0;

            var userTokens = GenerateTokens(user);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, userTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = user.Email;
            var toName = user.GetFullName();
            return SendNotification(messageTemplate, emailAccount,
                languageId, userTokens,
                toEmail, toName);
        }

        #endregion

        #region Newsletter workflow

        /// <summary>
        /// Sends a newsletter subscription activation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNewsLetterSubscriptionActivationMessage(NewsLetterSubscription subscription,
            int languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("NewsLetterSubscription.ActivationMessage", languageId);
            if (messageTemplate == null)
                return 0;

            var orderTokens = GenerateTokens(subscription);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, orderTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = subscription.Email;
            var toName = "";
            return SendNotification(messageTemplate, emailAccount,
                languageId, orderTokens,
                toEmail, toName);
        }

        /// <summary>
        /// Sends a newsletter subscription deactivation message
        /// </summary>
        /// <param name="subscription">Newsletter subscription</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNewsLetterSubscriptionDeactivationMessage(NewsLetterSubscription subscription,
            int languageId)
        {
            if (subscription == null)
                throw new ArgumentNullException("subscription");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("NewsLetterSubscription.DeactivationMessage", languageId);
            if (messageTemplate == null)
                return 0;

            var orderTokens = GenerateTokens(subscription);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, orderTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = subscription.Email;
            var toName = "";
            return SendNotification(messageTemplate, emailAccount,
                languageId, orderTokens,
                toEmail, toName);
        }

        #endregion

        #region Forum Notifications

        /// <summary>
        /// Sends a forum subscription message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public int SendNewForumTopicMessage(User user,
            ForumTopic forumTopic, Forum forum, int languageId)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var messageTemplate = GetLocalizedActiveMessageTemplate("Forums.NewForumTopic", languageId);
            if (messageTemplate == null || !messageTemplate.IsActive)
            {
                return 0;
            }

            var tokens = GenerateTokens(forumTopic);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = user.Email;
            var toName = user.GetFullName();

            return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
        }

        /// <summary>
        /// Sends a forum subscription message to a user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <param name="forumPost">Forum post</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="friendlyForumTopicPageIndex">Friendly (starts with 1) forum topic page to use for URL generation</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public int SendNewForumPostMessage(User user,
            ForumPost forumPost, ForumTopic forumTopic,
            Forum forum, int friendlyForumTopicPageIndex, int languageId)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var messageTemplate = GetLocalizedActiveMessageTemplate("Forums.NewForumPost", languageId);
            if (messageTemplate == null || !messageTemplate.IsActive)
            {
                return 0;
            }

            var tokens = GenerateTokens(forumPost, friendlyForumTopicPageIndex, forumPost.Id);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, tokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);            
            var toEmail = user.Email;
            var toName = user.GetFullName();

            return SendNotification(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
        }

        /// <summary>
        /// Sends a private message notification
        /// </summary>
        /// <param name="privateMessage">Private message</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public int SendPrivateMessageNotification(PrivateMessage privateMessage, int languageId)
        {
            if (privateMessage == null)
            {
                throw new ArgumentNullException("privateMessage");
            }

            var messageTemplate = GetLocalizedActiveMessageTemplate("User.NewPM", languageId);
            if (messageTemplate == null || !messageTemplate.IsActive)
            {
                return 0;
            }

            var privateMessageTokens = GenerateTokens(privateMessage);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, privateMessageTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);            
            var toEmail = privateMessage.ToUser.Email;
            var toName = privateMessage.ToUser.GetFullName();

            return SendNotification(messageTemplate, emailAccount, languageId, privateMessageTokens, toEmail, toName);
        }

        #endregion

        #region Misc

        /// <summary>
        /// Sends a "new VAT sumitted" notification to a site owner
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="vatName">Received VAT name</param>
        /// <param name="vatAddress">Received VAT address</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNewVatSubmittedSiteOwnerNotification(User user,
            string vatName, string vatAddress, int languageId)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("NewVATSubmitted.SiteOwnerNotification", languageId);
            if (messageTemplate == null)
                return 0;

            var vatSubmittedTokens = GenerateTokens(user);
            vatSubmittedTokens.Add(new Token("VatValidationResult.Name", vatName));
            vatSubmittedTokens.Add(new Token("VatValidationResult.Address", vatAddress));

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, vatSubmittedTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;
            return SendNotification(messageTemplate, emailAccount,
                languageId, vatSubmittedTokens,
                toEmail, toName);
        }

        /// <summary>
        /// Sends a news comment notification message to a site owner
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public virtual int SendNewsCommentNotificationMessage(NewsComment newsComment, int languageId)
        {
            if (newsComment == null)
                throw new ArgumentNullException("newsComment");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("News.NewsComment", languageId);
            if (messageTemplate == null)
                return 0;

            var newsCommentTokens = GenerateTokens(newsComment);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, newsCommentTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = emailAccount.Email;
            var toName = emailAccount.DisplayName;
            return SendNotification(messageTemplate, emailAccount,
                languageId, newsCommentTokens,
                toEmail, toName);
        }

        #endregion

        #endregion

        #region RFQ Workflow

        private IList<Token> GenerateTokens(SSG.Core.Domain.RFQ.RFQ rfq)
        {
            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddRFQTokens(tokens, rfq);
            return tokens;
        }

        private IList<Token> GenerateTokens(List<RFQLine> rfqLine)
        {
            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            //_messageTokenProvider.AddRFQTokens(tokens, rfqLine.RFQ);
            _messageTokenProvider.AddOpenRFQReportTokens(tokens);

            return tokens;
        }

        private IList<Token> GenerateTokens(SSG.Core.Domain.RFQ.RFQLine rfqLine)
        {
            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            _messageTokenProvider.AddRFQTokens(tokens, rfqLine.RFQ);
            _messageTokenProvider.AddRFQLineTokens(tokens, rfqLine);

            return tokens;
        }

        public int SendQuotationUploadedNotificationMessage(SSG.Core.Domain.RFQ.RFQLine rfqLine, int quotationNo, int languageId)
        {
            if (rfqLine == null)
                throw new ArgumentNullException("rfqLine");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("QUOTATION.Uploaded", languageId);

            if (messageTemplate == null)
                return 0;

            var rfqLineTokens = GenerateTokens(rfqLine);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, rfqLineTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            var toEmail = rfqLine.RFQ.Requestor.Email;
            var toName = rfqLine.RFQ.Requestor.GetFullName();

            //var toEmail = "antonio-amiel.yap@teradyne.com";
            //var toName = "Antonio Amiel L. Yap";

            var attachments = new StringBuilder();
            attachments.Append(rfqLine.RFQ.EmailAttachmentFilename);
            attachments.Append(";");

            var newQuotation = rfqLine.Quotations.Where(x => x.QuotationNo == quotationNo).FirstOrDefault();

            if (newQuotation != null)
            {
                foreach (var attachment in newQuotation.QuoteAttachment)
                {
                    attachments.Append(Path.Combine(rfqLine.QuotationAttachmentDir, attachment.Filename));
                    attachments.Append(";");
                }

                foreach (var attachment in newQuotation.ROHSDocumentAttachment)
                {
                    attachments.Append(Path.Combine(rfqLine.ROHSAttachmentDir, attachment.Filename));
                    attachments.Append(";");
                }

                foreach (var attachment in newQuotation.MSDSDocumentAttachment)
                {
                    attachments.Append(Path.Combine(rfqLine.MSDSAttachmentDir, attachment.Filename));
                    attachments.Append(";");
                }

                foreach (var attachment in newQuotation.OtherAttachments)
                {
                    attachments.Append(Path.Combine(rfqLine.OtherAttachmentDir, attachment.Filename));
                    attachments.Append(";");
                }
            }

            return SendNotification(messageTemplate, emailAccount,
                languageId, rfqLineTokens,
                toEmail, toName, attachments.ToString());
        }

        public int SendRFQLineDeletedNotificationMessage(SSG.Core.Domain.RFQ.RFQLine rfqLine, int languageId, bool forBuyer)
        {
            if (rfqLine == null)
                throw new ArgumentNullException("rfqLine");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("RFQLine.Deleted", languageId);

            if (messageTemplate == null)
                return 0;

            var rfqLineTokens = GenerateTokens(rfqLine);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, rfqLineTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = string.Empty;
            var toName = string.Empty;

            if (forBuyer)
            {
                toEmail = rfqLine.RFQ.Buyer.Email;
                toName = rfqLine.RFQ.Buyer.GetFullName();
            }
            else
            {
                toEmail = rfqLine.RFQ.Requestor.Email;
                toName = rfqLine.RFQ.Requestor.GetFullName();
            }
            
            //var toEmail = "antonio-amiel.yap@teradyne.com";
            //var toName = "Antonio Amiel L. Yap";

            return SendNotification(messageTemplate, emailAccount,
                languageId, rfqLineTokens,
                toEmail, toName, rfqLine.RFQ.EmailAttachmentFilename);
        }

        public int SendNoteToBuyerChangedNotificationMessage(SSG.Core.Domain.RFQ.RFQLine rfqLine, int languageId)
        {
            if (rfqLine == null)
                throw new ArgumentNullException("rfqLine");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("RFQLine.NoteToBuyerChanged", languageId);

            if (messageTemplate == null)
                return 0;

            var rfqLineTokens = GenerateTokens(rfqLine);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, rfqLineTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = rfqLine.RFQ.Buyer.Email;
            var toName = rfqLine.RFQ.Buyer.GetFullName();

            //var toEmail = "antonio-amiel.yap@teradyne.com";
            //var toName = "Antonio Amiel L. Yap";

            return SendNotification(messageTemplate, emailAccount,
                languageId, rfqLineTokens,
                toEmail, toName, rfqLine.RFQ.EmailAttachmentFilename);

        }

        public int SendBuyerChangedRFQNotificationMessage(SSG.Core.Domain.RFQ.RFQ rfq, int languageId)
        {
            if (rfq == null)
                throw new ArgumentNullException("rfq");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate("RFQ.BuyerChanged", languageId);

            if (messageTemplate == null)
                return 0;

            var rfqTokens = GenerateTokens(rfq);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, rfqTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = rfq.Buyer.Email;
            var toName = rfq.Buyer.GetFullName();

            //var toEmail = "antonio-amiel.yap@teradyne.com";
            //var toName = "Antonio Amiel L. Yap";

            return SendNotification(messageTemplate, emailAccount,
                languageId, rfqTokens,
                toEmail, toName, rfq.EmailAttachmentFilename);
        }

        public int SendSubmittedRFQNotificationMessage(SSG.Core.Domain.RFQ.RFQ rfq, int languageId, bool forBuyer)
        {
            if (rfq == null)
                throw new ArgumentNullException("rfq");

            languageId = EnsureLanguageIsActive(languageId);

            var messageTemplate = GetLocalizedActiveMessageTemplate(forBuyer ? "RFQ.SubmittedRFQForBuyer" : "RFQ.SubmittedRFQForRequestor", languageId);

            if (messageTemplate == null)
                return 0;

            var rfqTokens = GenerateTokens(rfq);

            //event notification
            _eventPublisher.MessageTokensAdded(messageTemplate, rfqTokens);

            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);
            var toEmail = forBuyer ? rfq.Buyer.Email : rfq.Requestor.Email;
            var toName = forBuyer ? rfq.Buyer.GetFullName() : rfq.Requestor.GetFullName();

            //var toEmail = "antonio-amiel.yap@teradyne.com";
            //var toName = "Antonio Amiel L. Yap";
            
            return SendNotification(messageTemplate, emailAccount,
                languageId, rfqTokens,
                toEmail, toName, rfq.EmailAttachmentFilename);
        }

        public int SendEmailForBuyerEveryMonday(List<RFQLine> rfqLine, int languageId,string attachmentFile)
        {
            if (rfqLine == null)
            {
                throw new ArgumentNullException("rfqLine");
            }
            languageId = EnsureLanguageIsActive(languageId);
            var messageTemplate = GetLocalizedActiveMessageTemplate("RFQLine.SendEveryMonday",languageId);
            if (messageTemplate == null)
            {
                return 0;
            }
            var rfqTokens = GenerateTokens(rfqLine);

            _eventPublisher.MessageTokensAdded(messageTemplate, rfqTokens);
            var emailAccount = GetEmailAccountOfMessageTemplate(messageTemplate, languageId);

            //var toEmail = "rheynold.bontog@teradyne.com";
            //var toName = "Rheynold Bontog";

            var toNameBuyer = new StringBuilder();
            var toEmailBuyer = new StringBuilder();

            var toEmail = toEmailBuyer.ToString();
            var toName = toNameBuyer.ToString();

            var buyersRoleIds = new[] { _userService.GetUserRoleBySystemName(SystemUserRoleNames.Buyer).Id };
            var buyers = _userService.GetUsersByUserRoleId(buyersRoleIds[0]).ToList();

            foreach (var buyer in buyers)
            {
                toNameBuyer.Append(buyer.GetFullName());
                toNameBuyer.Append(";");

            }
            toNameBuyer.Append("Rheynold Bontog");
            toNameBuyer.Append(";");
            toNameBuyer.Append("Antonio Amiel L. Yap");

            foreach (var buyer in buyers)
            {
                toEmailBuyer.Append(buyer.Email);
                toEmailBuyer.Append(";");
            }
            toEmailBuyer.Append("rheynold.bontog@teradyne.com");
            toEmailBuyer.Append(";");
            toEmailBuyer.Append("antonio-amiel.yap@teradyne.com");   

            return SendNotification(messageTemplate, emailAccount,
                languageId, rfqTokens,
                toEmail, toName, attachmentFile);
        }
        #endregion
    }
}
