using System.Collections.Generic;
using SSG.Web.Models.Common;

namespace SSG.Web.Models.User
{
    public partial class UserForumSubscriptionsModel
    {
        public UserForumSubscriptionsModel()
        {
            this.ForumSubscriptions = new List<ForumSubscriptionModel>();
        }

        public IList<ForumSubscriptionModel> ForumSubscriptions { get; set; }
        public UserNavigationModel NavigationModel { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}