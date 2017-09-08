//Contributor: Nicolas Muniere


using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel.Activation;
using SSG.Core;
using SSG.Core.Domain.Directory;
using SSG.Core.Domain.Users;
using SSG.Core.Infrastructure;
using SSG.Core.Plugins;
using SSG.Plugin.Misc.WebServices.Security;
using SSG.Services.Authentication;
using SSG.Services.Common;
using SSG.Services.Directory;
using SSG.Services.Security;
using SSG.Services.Users;

namespace SSG.Plugin.Misc.WebServices
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SSGService : ISSGService
    {
        #region Fields

        private readonly IAddressService _addressService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IUserService _userService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly UserSettings _userSettings;
        private readonly IPermissionService _permissionSettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly IPluginFinder _pluginFinder;
        
        #endregion 

        #region Ctor

        public SSGService()
        {
            _addressService = EngineContext.Current.Resolve<IAddressService>();
            _countryService = EngineContext.Current.Resolve<ICountryService>();
            _stateProvinceService = EngineContext.Current.Resolve<IStateProvinceService>();
            _userService = EngineContext.Current.Resolve<IUserService>();
            _userRegistrationService = EngineContext.Current.Resolve<IUserRegistrationService>();
            _userSettings = EngineContext.Current.Resolve<UserSettings>();
            _permissionSettings = EngineContext.Current.Resolve<IPermissionService>();
            _authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
            _workContext = EngineContext.Current.Resolve<IWorkContext>();
            _pluginFinder = EngineContext.Current.Resolve<IPluginFinder>();
        }

        #endregion 

        #region Utilities

        protected void CheckAccess(string usernameOrEmail, string userPassword)
        {
            //check whether web service plugin is installed
            var pluginDescriptor = _pluginFinder.GetPluginDescriptorBySystemName("Misc.WebServices");
            if (pluginDescriptor == null)
                throw new ApplicationException("Web services plugin cannot be loaded");

            if (!_userRegistrationService.ValidateUser(usernameOrEmail, userPassword))
                    throw new ApplicationException("Not allowed");
            
            var user = _userSettings.UsernamesEnabled ? _userService.GetUserByUsername(usernameOrEmail) : _userService.GetUserByEmail(usernameOrEmail);

            _workContext.CurrentUser = user;
            _authenticationService.SignIn(user, true);

            //valdiate whether we can access this web service
            if (!_permissionSettings.Authorize(WebServicePermissionProvider.AccessWebService))
                throw new ApplicationException("Not allowed to access web service");
        }

        #endregion
    }
}
