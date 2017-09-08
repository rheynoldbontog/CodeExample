using SSG.Core.Configuration;

namespace SSG.Plugin.ExternalAuth.Facebook
{
    public class FacebookExternalAuthSettings : ISettings
    {
        public string ClientKeyIdentifier { get; set; }
        public string ClientSecret { get; set; }
    }
}
