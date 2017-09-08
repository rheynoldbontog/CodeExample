using SSG.Admin.Models.Common;
using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.Users
{
    public partial class UserAddressModel : BaseSSGModel
    {
        public int UserId { get; set; }

        public AddressModel Address { get; set; }
    }
}