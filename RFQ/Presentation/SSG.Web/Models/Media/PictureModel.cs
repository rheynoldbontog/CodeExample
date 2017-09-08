using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.Media
{
    public partial class PictureModel : BaseSSGModel
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}