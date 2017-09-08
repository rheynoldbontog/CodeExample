using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Common
{
    public partial class FaviconModel : BaseSSGModel
    {
        public bool Uploaded { get; set; }
        public string FaviconUrl { get; set; }
    }
}