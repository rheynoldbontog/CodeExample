using SSG.Web.Framework.Mvc;
using SSG.Web.Models.Common;

namespace SSG.Web.Models.User
{
    public partial class UserAddressEditModel : BaseSSGModel
    {
        public AddressModel Address { get; set; }
        public UserNavigationModel NavigationModel { get; set; }
    }
}