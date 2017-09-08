using System;
using System.Web.Mvc;
using SSG.Web.Framework;
using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.News
{
    public partial class NewsCommentModel : BaseSSGEntityModel
    {
        [SSGResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.NewsItem")]
        public int NewsItemId { get; set; }
        [SSGResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.NewsItem")]
        [AllowHtml]
        public string NewsItemTitle { get; set; }

        [SSGResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.User")]
        public int UserId { get; set; }

        [SSGResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.IPAddress")]
        public string IpAddress { get; set; }

        [AllowHtml]
        [SSGResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.CommentTitle")]
        public string CommentTitle { get; set; }

        [AllowHtml]
        [SSGResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.CommentText")]
        public string CommentText { get; set; }

        [SSGResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

    }
}