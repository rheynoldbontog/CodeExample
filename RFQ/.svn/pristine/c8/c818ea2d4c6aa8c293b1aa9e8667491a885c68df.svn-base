using SSG.Web.Framework.Mvc;

namespace SSG.Web.Models.User
{
    public partial class UserNavigationModel : BaseSSGModel
    {
        public bool HideInfo { get; set; }
        public bool HideAddresses { get; set; }
        public bool HideChangePassword { get; set; }
        public bool HideAvatar { get; set; }
        public bool HideForumSubscriptions { get; set; }

        public UserNavigationEnum SelectedTab { get; set; }
    }

    public enum UserNavigationEnum
    {
        Info,
        Addresses,
        ChangePassword,
        Avatar,
        ForumSubscriptions
    }
}