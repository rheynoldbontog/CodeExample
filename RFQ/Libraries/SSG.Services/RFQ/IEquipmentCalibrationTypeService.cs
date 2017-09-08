using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IEquipmentCalibrationTypeService
    {
        IEnumerable<EquipmentCalibrationType> GetAllEquipmentCalibrationTypes(bool activeOnly = false);
        IQueryable<EquipmentCalibrationType> GetAllEquipmentCalibrationTypeQueryable(bool activeOnly = false);
        void SaveEquipmentCalibrationType(EquipmentCalibrationType equipmentCalibrationType);
        IQueryable<EquipmentCalibrationType> GetPagedEquipmentCalibrationTypes(int currentPage, int pageSize, out int totalCount);
        EquipmentCalibrationType GetEquipmentCalibrationTypeById(int id);
    }
}
