using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using SSG.Services.Forums;
using SSG.Services.News;
using SSG.Services.Topics;
using SSG.Services.Users;
using SSG.Services.Seo;

namespace SSG.Web.Controllers
{
    public partial class BackwardCompatibility1XController : BaseSSGController
    {
		#region Fields

        private readonly INewsService _newsService;
        private readonly ITopicService _topicService;
        private readonly IForumService _forumService;
        private readonly IUserService _userService;
        #endregion

		#region Constructors

        public BackwardCompatibility1XController(INewsService newsService,
            ITopicService topicService,
            IForumService forumService, IUserService userService)
        {
            this._newsService = newsService;
            this._topicService = topicService;
            this._forumService = forumService;
            this._userService = userService;
        }

		#endregion
        
        #region Methods

        public virtual ActionResult GeneralRedirect()
        {
            
            // use Request.RawUrl, for instance to parse out what was invoked
            // this regex will extract anything between a "/" and a ".aspx"
            var regex = new Regex(@"(?<=/).+(?=\.aspx)", RegexOptions.Compiled);
            var aspxfileName = regex.Match(Request.RawUrl).Value.ToLowerInvariant();


            switch (aspxfileName)
            {
                //URL without rewriting
                case "news":
                    {
                        return RedirectNewsItem(Request.QueryString["newsid"], false);
                    }
                case "topic":
                    {
                        return RedirectTopic(Request.QueryString["topicid"], false);
                    }
                case "profile":
                    {
                        return RedirectUserProfile(Request.QueryString["UserId"]);
                    }
                case "contactus":
                    {
                        return RedirectToRoutePermanent("ContactUs");
                    }
                case "passwordrecovery":
                    {
                        return RedirectToRoutePermanent("PasswordRecovery");
                    }
                case "login":
                    {
                        return RedirectToRoutePermanent("Login");
                    }
                case "register":
                    {
                        return RedirectToRoutePermanent("Register");
                    }
                case "newsarchive":
                    {
                        return RedirectToRoutePermanent("NewsArchive");
                    }
                case "sitemap":
                    {
                        return RedirectToRoutePermanent("Sitemap");
                    }
                case "sitemapseo":
                    {
                        return RedirectToRoutePermanent("SitemapSEO");
                    }
                default:
                    break;
            }

            //no permanent redirect in this case
            return RedirectToRoute("HomePage");
        }

        public virtual ActionResult RedirectNewsItem(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            var newsId = idIncludesSename ? Convert.ToInt32(id.Split(new char[] { '-' })[0]) : Convert.ToInt32(id);
            var newsItem = _newsService.GetNewsById(newsId);
            if (newsItem == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("NewsItem", new { newsItemId = newsItem.Id, SeName = newsItem.GetSeName() });
        }

        public virtual ActionResult RedirectTopic(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            var topicid = idIncludesSename ? Convert.ToInt32(id.Split(new char[] { '-' })[0]) : Convert.ToInt32(id);
            var topic = _topicService.GetTopicById(topicid);
            if (topic == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("Topic", new { systemName = topic.SystemName });
        }

        public virtual ActionResult RedirectForumGroup(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            var forumGroupId = idIncludesSename ? Convert.ToInt32(id.Split(new char[] { '-' })[0]) : Convert.ToInt32(id);
            var forumGroup = _forumService.GetForumGroupById(forumGroupId);
            if (forumGroup == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("ForumGroupSlug", new { id = forumGroup.Id, slug = forumGroup.GetSeName() });
        }

        public virtual ActionResult RedirectForum(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            var forumId = idIncludesSename ? Convert.ToInt32(id.Split(new char[] { '-' })[0]) : Convert.ToInt32(id);
            var forum = _forumService.GetForumById(forumId);
            if (forum == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("ForumSlug", new { id = forum.Id, slug = forum.GetSeName() });
        }

        public virtual ActionResult RedirectForumTopic(string id, bool idIncludesSename = true)
        {
            //we can't use dash in MVC
            var forumTopicId = idIncludesSename ? Convert.ToInt32(id.Split(new char[] { '-' })[0]) : Convert.ToInt32(id);
            var topic = _forumService.GetTopicById(forumTopicId);
            if (topic == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("TopicSlug", new { id = topic.Id, slug = topic.GetSeName() });
        }

        public virtual ActionResult RedirectUserProfile(string id)
        {
            //we can't use dash in MVC
            var userId = Convert.ToInt32(id);
            var user = _userService.GetUserById(userId);
            if (user == null)
                return RedirectToRoutePermanent("HomePage");

            return RedirectToRoutePermanent("UserProfile", new { id = user.Id});
        }
        
        #endregion
    }
}
