﻿using System;
using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Domain.Users;
using SSG.Core.Domain.Messages;
using SSG.Services.Localization;
using SSG.Services.Messages;
using SSG.Web.Models.Newsletter;

namespace SSG.Web.Controllers
{
    public partial class NewsletterController : BaseSSGController
    {
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IWorkflowMessageService _workflowMessageService;

        private readonly UserSettings _userSettings;

        public NewsletterController(ILocalizationService localizationService,
            IWorkContext workContext, INewsLetterSubscriptionService newsLetterSubscriptionService,
            IWorkflowMessageService workflowMessageService, UserSettings userSettings)
        {
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._workflowMessageService = workflowMessageService;

            this._userSettings = userSettings;
        }

        [ChildActionOnly]
        public virtual ActionResult NewsletterBox()
        {
            if (_userSettings.HideNewsletterBlock)
                return Content("");

            return PartialView(new NewsletterBoxModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SubscribeNewsletter(bool subscribe, string email)
        {
            string result;
            bool success = false;

            if (!CommonHelper.IsValidEmail(email))
                result = _localizationService.GetResource("Newsletter.Email.Wrong");
            else
            {
                //subscribe/unsubscribe
                email = email.Trim();

                var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmail(email);
                if (subscription != null)
                {
                    if (subscribe)
                    {
                        if (!subscription.Active)
                        {
                            _workflowMessageService.SendNewsLetterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Newsletter.SubscribeEmailSent");
                    }
                    else
                    {
                        if (subscription.Active)
                        {
                            _workflowMessageService.SendNewsLetterSubscriptionDeactivationMessage(subscription, _workContext.WorkingLanguage.Id);
                        }
                        result = _localizationService.GetResource("Newsletter.UnsubscribeEmailSent");
                    }
                }
                else if (subscribe)
                {
                    subscription = new NewsLetterSubscription()
                    {
                        NewsLetterSubscriptionGuid = Guid.NewGuid(),
                        Email = email,
                        Active = false,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    _newsLetterSubscriptionService.InsertNewsLetterSubscription(subscription);
                    _workflowMessageService.SendNewsLetterSubscriptionActivationMessage(subscription, _workContext.WorkingLanguage.Id);

                    result = _localizationService.GetResource("Newsletter.SubscribeEmailSent");
                }
                else
                {
                    result = _localizationService.GetResource("Newsletter.UnsubscribeEmailSent");
                }
                success = true;
            }

            return Json(new
            {
                Success = success,
                Result = result,
            });
        }

        public virtual ActionResult SubscriptionActivation(Guid token, bool active)
        {
            var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByGuid(token);
            if (subscription == null)
                return RedirectToRoute("HomePage");

            var model = new SubscriptionActivationModel();

            if (active)
            {
                subscription.Active = active;
                _newsLetterSubscriptionService.UpdateNewsLetterSubscription(subscription);
            }
            else
                _newsLetterSubscriptionService.DeleteNewsLetterSubscription(subscription);

            if (active)
                model.Result = _localizationService.GetResource("Newsletter.ResultActivated");
            else
                model.Result = _localizationService.GetResource("Newsletter.ResultDeactivated");

            return View(model);
        }
    }
}
