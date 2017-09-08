using System.Web.Mvc;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.News
{
    public partial class AddNewsCommentModel : BaseSSGModel
    {
        [SSGResourceDisplayName("News.Comments.CommentTitle")]
        [AllowHtml]
        public string CommentTitle { get; set; }

        [SSGResourceDisplayName("News.Comments.CommentText")]
        [AllowHtml]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}