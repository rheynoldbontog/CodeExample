using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Common
{
    public partial class HeaderLinksModel : BaseSSGModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserEmailUsername { get; set; }
        public bool IsUserImpersonated { get; set; }

        public bool DisplayAdminLink { get; set; }

        public bool AllowPrivateMessages { get; set; }
        public string UnreadPrivateMessages { get; set; }
        public string AlertMessage { get; set; }
    }
}