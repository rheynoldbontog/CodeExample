using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.User
{
    public partial class UserAvatarModel : BaseSSGModel
    {
        public string AvatarUrl { get; set; }
        public UserNavigationModel NavigationModel { get; set; }
    }
}