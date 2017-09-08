using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Plugin.ExternalAuth.Twitter.Models
{
    public class ConfigurationModel : BaseSSGModel
    {
        [SSGResourceDisplayName("Plugins.ExternalAuth.Twitter.ConsumerKey")]
        public string ConsumerKey { get; set; }
        [SSGResourceDisplayName("Plugins.ExternalAuth.Twitter.ConsumerSecret")]
        public string ConsumerSecret { get; set; }
    }
}