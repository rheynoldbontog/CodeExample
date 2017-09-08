using SSG.Web.Framework.Mvc;

namespace SSG.Admin.Models.Security
{
    public partial class PermissionRecordModel : BaseSSGModel
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
    }
}