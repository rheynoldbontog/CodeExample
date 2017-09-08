using System;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using SSG.Core;
using SSG.Core.Domain;
using SSG.Core.Domain.Common;
using SSG.Services.Configuration;
using SSG.Web.Framework.Controllers;

namespace SSG.Admin.Controllers
{
    [AdminAuthorize]
    public partial class HomeController : BaseSSGController
    {
        #region Fields
        private readonly SiteInformationSettings _siteInformationSettings;
        private readonly CommonSettings _commonSettings;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public HomeController(SiteInformationSettings siteInformationSettings,
            CommonSettings commonSettings, ISettingService settingService)
        {
            this._siteInformationSettings = siteInformationSettings;
            this._commonSettings = commonSettings;
            this._settingService = settingService;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult SSGNews()
        {
            try
            {
                string feedUrl = string.Format("http://jeepme/webapp/NewsRSS.aspx?Version={0}&Localhost={1}&HideAdvertisements={2}&SiteURL={3}",
                    SSGVersion.CurrentVersion, 
                    Request.Url.IsLoopback, 
                    _commonSettings.HideAdvertisementsOnAdminArea, 
                    _siteInformationSettings.SiteUrl);

                //specify timeout (5 secs)
                var request = WebRequest.Create(feedUrl);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                using (var reader = XmlReader.Create(response.GetResponseStream()))
                {
                    var rssData = SyndicationFeed.Load(reader);
                    return PartialView(rssData);
                }
            }
            catch (Exception)
            {
                return Content("");
            }
        }

        [HttpPost]
        public ActionResult SSGNewsHideAdv()
        {
            _commonSettings.HideAdvertisementsOnAdminArea = !_commonSettings.HideAdvertisementsOnAdminArea;
            _settingService.SaveSetting(_commonSettings);
            return Content("Setting changed");
        }

        #endregion
    }
}
