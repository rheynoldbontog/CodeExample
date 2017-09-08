using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.EntityExtensions
{
    public static class BaseAuditEntityExtension
    {
        public static void PopulateAuditFieldsForSave(this BaseAuditEntity ent, int userId)
        {
            ent.DateCreatedOnUtc = DateTime.UtcNow;
            ent.DateUpdatedOnUtc = DateTime.UtcNow;
            ent.CreatedByUserId = userId;
            ent.UpdatedByUserId = userId;
        }

        public static void PopulateAuditFieldsForEdit(this BaseAuditEntity ent, int userId)
        {
            ent.DateUpdatedOnUtc = DateTime.UtcNow;
            ent.UpdatedByUserId = userId;
        }
    }
}
