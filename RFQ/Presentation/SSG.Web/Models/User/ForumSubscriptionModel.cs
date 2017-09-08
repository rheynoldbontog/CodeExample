using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.User
{
    public partial class ForumSubscriptionModel : BaseSSGEntityModel
    {
        public int ForumId { get; set; }
        public int ForumTopicId { get; set; }
        public bool TopicSubscription { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
    }
}
