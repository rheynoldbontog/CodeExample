using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IEquipmentPurchaseTypeService
    {
        IEnumerable<EquipmentPurchaseType> GetAllEquipmentPurchaseTypes(bool activeOnly = false);
        IQueryable<EquipmentPurchaseType> GetAllEquipmentPurchaseTypeQueryable(bool activeOnly = false);
        void SaveEquipmentPurchaseType(EquipmentPurchaseType equipmentPurchaseType);
        IQueryable<EquipmentPurchaseType> GetPagedEquipmentPurchaseTypes(int currentPage, int pageSize, out int totalCount);
        EquipmentPurchaseType GetEquipmentPurchaseTypeById(int id);
    }
}
