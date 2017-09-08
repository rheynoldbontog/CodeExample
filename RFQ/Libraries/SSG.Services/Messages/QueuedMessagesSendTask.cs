﻿using System;
using SSG.Services.Logging;
using SSG.Services.Tasks;

namespace SSG.Services.Messages
{
    /// <summary>
    /// Represents a task for sending queued message 
    /// </summary>
    public partial class QueuedMessagesSendTask : ITask
    {
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public QueuedMessagesSendTask(IQueuedEmailService queuedEmailService,
            IEmailSender emailSender, ILogger logger)
        {
            this._queuedEmailService = queuedEmailService;
            this._emailSender = emailSender;
            this._logger = logger;
        }

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var maxTries = 3;
            var queuedEmails = _queuedEmailService.SearchEmails(null, null, null, null,
                true, maxTries, false, 0, 10000);
            foreach (var queuedEmail in queuedEmails)
            {
                var bcc = String.IsNullOrWhiteSpace(queuedEmail.Bcc) 
                            ? null 
                            : queuedEmail.Bcc.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var cc = String.IsNullOrWhiteSpace(queuedEmail.CC) 
                            ? null 
                            : queuedEmail.CC.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                var attachments = String.IsNullOrWhiteSpace(queuedEmail.Attachments)
                            ? null
                            : queuedEmail.Attachments.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                
                var to = queuedEmail.To.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries); 
                try
                {
                    if (to.Length > 1)
                    {
                        _emailSender.SendEmailNew(queuedEmail.EmailAccount, queuedEmail.Subject, queuedEmail.Body,
                            new System.Net.Mail.MailAddress(queuedEmail.From, queuedEmail.FromName), to, bcc, cc,
                            attachments);
                    }
                    else
                    {
                        _emailSender.SendEmail(queuedEmail.EmailAccount, queuedEmail.Subject, queuedEmail.Body,
                            queuedEmail.From, queuedEmail.FromName, queuedEmail.To, queuedEmail.ToName, bcc, cc, attachments);
                    }
                  

                   
                    
                    queuedEmail.SentOnUtc = DateTime.UtcNow;
                }
                catch (Exception exc)
                {
                    _logger.Error(string.Format("Error sending e-mail. {0}", exc.Message), exc);
                }
                finally
                {
                    queuedEmail.SentTries = queuedEmail.SentTries + 1;
                    _queuedEmailService.UpdateQueuedEmail(queuedEmail);
                }
            }
        }
    }
}
