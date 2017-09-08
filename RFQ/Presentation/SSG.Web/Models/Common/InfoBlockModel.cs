using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Common
{
    public partial class InfoBlockModel : BaseSSGModel
    {
        public bool SitemapEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public bool AllowPrivateMessages { get; set; }
    }
}