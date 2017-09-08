using SSG.Web.Framework;

namespace SSG.Plugin.Sms.Clickatell.Models
{
    public class SmsClickatellModel
    {
        [SSGResourceDisplayName("Plugins.Sms.Clickatell.Fields.Enabled")]
        public bool Enabled { get; set; }

        [SSGResourceDisplayName("Plugins.Sms.Clickatell.Fields.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [SSGResourceDisplayName("Plugins.Sms.Clickatell.Fields.ApiId")]
        public string ApiId { get; set; }

        [SSGResourceDisplayName("Plugins.Sms.Clickatell.Fields.Username")]
        public string Username { get; set; }

        [SSGResourceDisplayName("Plugins.Sms.Clickatell.Fields.Password")]
        public string Password { get; set; }


        [SSGResourceDisplayName("Plugins.Sms.Clickatell.Fields.TestMessage")]
        public string TestMessage { get; set; }
        public string TestSmsResult { get; set; }
    }
}