using System.Collections.Generic;
using System.Web.Mvc;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Plugin.Widgets.LivePersonChat.Models
{
    public class ConfigurationModel : BaseSSGModel
    {
        public ConfigurationModel()
        {
            AvailableZones = new List<SelectListItem>();
        }

        [SSGResourceDisplayName("Admin.ContentManagement.Widgets.ChooseZone")]
        public string ZoneId { get; set; }
        public IList<SelectListItem> AvailableZones { get; set; }

        [SSGResourceDisplayName("Plugins.Widgets.LivePersonChat.ButtonCode")]
        [AllowHtml]
        public string ButtonCode { get; set; }

        [SSGResourceDisplayName("Plugins.Widgets.LivePersonChat.MonitoringCode")]
        [AllowHtml]
        public string MonitoringCode { get; set; }
    }
}