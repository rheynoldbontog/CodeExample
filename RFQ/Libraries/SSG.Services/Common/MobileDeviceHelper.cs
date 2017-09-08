using System.Web;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Core.Domain.Users;

namespace SSG.Services.Common
{
    /// <summary>
    /// Mobile device helper
    /// </summary>
    public partial class MobileDeviceHelper : IMobileDeviceHelper
    {
        #region Fields

        private readonly SiteInformationSettings _siteInformationSettings;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="siteInformationSettings">Site information settings</param>
        /// <param name="workContext">Work context</param>
        public MobileDeviceHelper(SiteInformationSettings siteInformationSettings,
            IWorkContext workContext)
        {
            this._siteInformationSettings = siteInformationSettings;
            this._workContext = workContext;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Returns a value indicating whether request is made by a mobile device
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <returns>Result</returns>
        public virtual bool IsMobileDevice(HttpContextBase httpContext)
        {
            return httpContext.Request.Browser.IsMobileDevice ||
                _siteInformationSettings.EmulateMobileDevice;
        }

        /// <summary>
        /// Returns a value indicating whether mobile devices support is enabled
        /// </summary>
        public virtual bool MobileDevicesSupported()
        {
            return _siteInformationSettings.MobileDevicesSupported;
        }

        /// <summary>
        /// Returns a value indicating whether current user prefer to use full desktop version (even request is made by a mobile device)
        /// </summary>
        public virtual bool UserDontUseMobileVersion()
        {
            return _workContext.CurrentUser.GetAttribute<bool>(SystemUserAttributeNames.DontUseMobileVersion);
        }

        #endregion
    }
}