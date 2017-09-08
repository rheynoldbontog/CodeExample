using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using SSG.Core.Data;
using SSG.Core.Domain.Messages;
using SSG.Services.Users;
using SSG.Services.Events;

namespace SSG.Services.Messages
{
    public partial class CampaignService : ICampaignService
    {
        private readonly IRepository<Campaign> _campaignRepository;
        private readonly IEmailSender _emailSender;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly ITokenizer _tokenizer;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="campaignRepository">Campaign repository</param>
        /// <param name="emailSender">Email sender</param>
        /// <param name="messageTokenProvider">Message token provider</param>
        /// <param name="tokenizer">Tokenizer</param>
        /// <param name="queuedEmailService">Queued email service</param>
        /// <param name="userService">User service</param>
        /// <param name="eventPublisher">Event published</param>
        public CampaignService(IRepository<Campaign> campaignRepository,
            IEmailSender emailSender, IMessageTokenProvider messageTokenProvider,
            ITokenizer tokenizer, IQueuedEmailService queuedEmailService,
            IUserService userService, IEventPublisher eventPublisher)
        {
            this._campaignRepository = campaignRepository;
            this._emailSender = emailSender;
            this._messageTokenProvider = messageTokenProvider;
            this._tokenizer = tokenizer;
            this._queuedEmailService = queuedEmailService;
            this._userService = userService;
            this._eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Inserts a campaign
        /// </summary>
        /// <param name="campaign">Campaign</param>        
        public virtual void InsertCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            _campaignRepository.Insert(campaign);

            //event notification
            _eventPublisher.EntityInserted(campaign);
        }

        /// <summary>
        /// Updates a campaign
        /// </summary>
        /// <param name="campaign">Campaign</param>
        public virtual void UpdateCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            _campaignRepository.Update(campaign);

            //event notification
            _eventPublisher.EntityUpdated(campaign);
        }

        /// <summary>
        /// Deleted a queued email
        /// </summary>
        /// <param name="campaign">Campaign</param>
        public virtual void DeleteCampaign(Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            _campaignRepository.Delete(campaign);

            //event notification
            _eventPublisher.EntityDeleted(campaign);
        }

        /// <summary>
        /// Gets a campaign by identifier
        /// </summary>
        /// <param name="campaignId">Campaign identifier</param>
        /// <returns>Campaign</returns>
        public virtual Campaign GetCampaignById(int campaignId)
        {
            if (campaignId == 0)
                return null;

            var campaign = _campaignRepository.GetById(campaignId);
            return campaign;

        }

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>Campaign collection</returns>
        public virtual IList<Campaign> GetAllCampaigns()
        {

            var query = from c in _campaignRepository.Table
                        orderby c.CreatedOnUtc
                        select c;
            var campaigns = query.ToList();

            return campaigns;
        }
        
        /// <summary>
        /// Sends a campaign to specified emails
        /// </summary>
        /// <param name="campaign">Campaign</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="subscriptions">Subscriptions</param>
        /// <returns>Total emails sent</returns>
        public virtual int SendCampaign(Campaign campaign, EmailAccount emailAccount,
            IEnumerable<NewsLetterSubscription> subscriptions)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");

            int totalEmailsSent = 0;

            foreach (var subscription in subscriptions)
            {
                var tokens = new List<Token>();
                _messageTokenProvider.AddSiteTokens(tokens);
                _messageTokenProvider.AddNewsLetterSubscriptionTokens(tokens, subscription);
                var user = _userService.GetUserByEmail(subscription.Email);
                if (user != null)
                    _messageTokenProvider.AddUserTokens(tokens, user);

                string subject = _tokenizer.Replace(campaign.Subject, tokens, false);
                string body = _tokenizer.Replace(campaign.Body, tokens, true);

                var email = new QueuedEmail()
                {
                    Priority = 3,
                    From = emailAccount.Email,
                    FromName = emailAccount.DisplayName,
                    To = subscription.Email,
                    Subject = subject,
                    Body = body,
                    CreatedOnUtc = DateTime.UtcNow,
                    EmailAccountId = emailAccount.Id
                };
                _queuedEmailService.InsertQueuedEmail(email);
                totalEmailsSent++;
            }
            return totalEmailsSent;
        }

        /// <summary>
        /// Sends a campaign to specified email
        /// </summary>
        /// <param name="campaign">Campaign</param>
        /// <param name="emailAccount">Email account</param>
        /// <param name="email">Email</param>
        public virtual void SendCampaign(Campaign campaign, EmailAccount emailAccount, string email)
        {
            if (campaign == null)
                throw new ArgumentNullException("campaign");

            if (emailAccount == null)
                throw new ArgumentNullException("emailAccount");

            var tokens = new List<Token>();
            _messageTokenProvider.AddSiteTokens(tokens);
            var user = _userService.GetUserByEmail(email);
            if (user != null)
                _messageTokenProvider.AddUserTokens(tokens, user);
            
            string subject = _tokenizer.Replace(campaign.Subject, tokens, false);
            string body = _tokenizer.Replace(campaign.Body, tokens, true);

            var from = new MailAddress(emailAccount.Email, emailAccount.DisplayName);
            var to = new MailAddress(email);
            _emailSender.SendEmail(emailAccount, subject, body, from, to);
        }
    }
}
