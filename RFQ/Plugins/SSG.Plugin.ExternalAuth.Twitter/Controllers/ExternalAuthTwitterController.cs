using System.Web.Mvc;
using SSG.Core;
using SSG.Core.Domain.Users;
using SSG.Plugin.ExternalAuth.Twitter.Core;
using SSG.Plugin.ExternalAuth.Twitter.Models;
using SSG.Services.Authentication.External;
using SSG.Services.Configuration;
using SSG.Web.Framework;
using SSG.Web.Framework.Controllers;

namespace SSG.Plugin.ExternalAuth.Twitter.Controllers
{
    public class ExternalAuthTwitterController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly TwitterExternalAuthSettings _twitterExternalAuthSettings;
        private readonly IOAuthProviderTwitterAuthorizer _oAuthProviderTwitterAuthorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;

        public ExternalAuthTwitterController(ISettingService settingService,
            TwitterExternalAuthSettings twitterExternalAuthSettings,
            IOAuthProviderTwitterAuthorizer oAuthProviderTwitterAuthorizer,
            IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings)
        {
            this._settingService = settingService;
            this._twitterExternalAuthSettings = twitterExternalAuthSettings;
            this._oAuthProviderTwitterAuthorizer = oAuthProviderTwitterAuthorizer;
            this._openAuthenticationService = openAuthenticationService;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
        }
        
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new ConfigurationModel();
            model.ConsumerKey = _twitterExternalAuthSettings.ConsumerKey;
            model.ConsumerSecret = _twitterExternalAuthSettings.ConsumerSecret;
            
            return View("SSG.Plugin.ExternalAuth.Twitter.Views.ExternalAuthTwitter.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();
            
            //save settings
            _twitterExternalAuthSettings.ConsumerKey = model.ConsumerKey;
            _twitterExternalAuthSettings.ConsumerSecret = model.ConsumerSecret;
            _settingService.SaveSetting(_twitterExternalAuthSettings);
            
            return View("SSG.Plugin.ExternalAuth.Twitter.Views.ExternalAuthTwitter.Configure", model);
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View("SSG.Plugin.ExternalAuth.Twitter.Views.ExternalAuthTwitter.PublicInfo");
        }


        public ActionResult Login(string returnUrl)
        {
            var processor = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName("ExternalAuth.Twitter");
            if (processor == null ||
                !processor.IsMethodActive(_externalAuthenticationSettings) || !processor.PluginDescriptor.Installed)
                throw new SSGException("Twitter module cannot be loaded");

            var viewModel = new LoginModel();
            TryUpdateModel(viewModel);

            var result = _oAuthProviderTwitterAuthorizer.Authorize(returnUrl);
            switch (result.AuthenticationStatus)
            {
                case OpenAuthenticationStatus.Error:
                    {
                        if (!result.Success)
                            foreach (var error in result.Errors)
                                ExternalAuthorizerHelper.AddErrorsToDisplay(error);

                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AssociateOnLogon:
                    {
                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AutoRegisteredEmailValidation:
                    {
                        //result
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation });
                    }
                case OpenAuthenticationStatus.AutoRegisteredAdminApproval:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.AdminApproval });
                    }
                case OpenAuthenticationStatus.AutoRegisteredStandard:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                    }
                default:
                    break;
            }

            if (result.Result != null) return result.Result;
            return HttpContext.Request.IsAuthenticated ? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
    }
}