using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Plugin.ExternalAuth.Facebook.Models
{
    public class ConfigurationModel : BaseSSGModel
    {
        [SSGResourceDisplayName("Plugins.ExternalAuth.Facebook.ClientKeyIdentifier")]
        public string ClientKeyIdentifier { get; set; }
        [SSGResourceDisplayName("Plugins.ExternalAuth.Facebook.ClientSecret")]
        public string ClientSecret { get; set; }
    }
}