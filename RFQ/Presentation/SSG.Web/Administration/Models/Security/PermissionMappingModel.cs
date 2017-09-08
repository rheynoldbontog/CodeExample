using System.Collections.Generic;
using SSG.Admin.Models.Users;
using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.Security
{
    public partial class PermissionMappingModel : BaseSSGModel
    {
        public PermissionMappingModel()
        {
            AvailablePermissions = new List<PermissionRecordModel>();
            AvailableUserRoles = new List<UserRoleModel>();
            Allowed = new Dictionary<string, IDictionary<int, bool>>();
        }
        public IList<PermissionRecordModel> AvailablePermissions { get; set; }
        public IList<UserRoleModel> AvailableUserRoles { get; set; }

        //[permission system name] / [user role id] / [allowed]
        public IDictionary<string, IDictionary<int, bool>> Allowed { get; set; }
    }
}