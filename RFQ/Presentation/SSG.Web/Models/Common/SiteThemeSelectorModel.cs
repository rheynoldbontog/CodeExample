using System.Collections.Generic;
using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Common
{
    public partial class SiteThemeSelectorModel : BaseSSGModel
    {
        public SiteThemeSelectorModel()
        {
            AvailableSiteThemes = new List<SiteThemeModel>();
        }

        public IList<SiteThemeModel> AvailableSiteThemes { get; set; }

        public SiteThemeModel CurrentSiteTheme { get; set; }
    }
}