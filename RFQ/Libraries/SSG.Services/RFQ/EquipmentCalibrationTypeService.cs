using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class EquipmentCalibrationTypeService : IEquipmentCalibrationTypeService
    {
        #region Fields
        
        private readonly IRepository<EquipmentCalibrationType> _equipmentCalibrationTypeRepository;
        
        #endregion

        #region ctor

        public EquipmentCalibrationTypeService(IRepository<EquipmentCalibrationType> equipmentCalibrationTypeRepository)
        {
            this._equipmentCalibrationTypeRepository = equipmentCalibrationTypeRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<EquipmentCalibrationType> GetAllEquipmentCalibrationTypes(bool activeOnly = false)
        {
            return this._equipmentCalibrationTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<EquipmentCalibrationType> GetAllEquipmentCalibrationTypeQueryable(bool activeOnly = false)
        {
            return this._equipmentCalibrationTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveEquipmentCalibrationType(EquipmentCalibrationType equipmentCalibrationType)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (equipmentCalibrationType.Id == 0)
                    {
                        this._equipmentCalibrationTypeRepository.Insert(equipmentCalibrationType);
                    }
                    else
                    {
                        this._equipmentCalibrationTypeRepository.Update(equipmentCalibrationType);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<EquipmentCalibrationType> GetPagedEquipmentCalibrationTypes(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._equipmentCalibrationTypeRepository.Table.OrderBy(s => s.Name);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public EquipmentCalibrationType GetEquipmentCalibrationTypeById(int id)
        {
            return this._equipmentCalibrationTypeRepository.GetById(id);
        }

        #endregion
    }
}
