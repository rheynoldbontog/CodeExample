using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class EquipmentPurchaseTypeService : IEquipmentPurchaseTypeService
    {
        #region Fields

        private readonly IRepository<EquipmentPurchaseType> _equipmentPurchaseTypeRepository;

        #endregion

        #region ctor

        public EquipmentPurchaseTypeService(IRepository<EquipmentPurchaseType> equipmentPurchaseTypeRepository)
        {
            this._equipmentPurchaseTypeRepository = equipmentPurchaseTypeRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<EquipmentPurchaseType> GetAllEquipmentPurchaseTypes(bool activeOnly = false)
        {
            return this._equipmentPurchaseTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<EquipmentPurchaseType> GetAllEquipmentPurchaseTypeQueryable(bool activeOnly = false)
        {
            return this._equipmentPurchaseTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveEquipmentPurchaseType(EquipmentPurchaseType equipmentPurchaseType)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (equipmentPurchaseType.Id == 0)
                    {
                        this._equipmentPurchaseTypeRepository.Insert(equipmentPurchaseType);
                    }
                    else
                    {
                        this._equipmentPurchaseTypeRepository.Update(equipmentPurchaseType);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<EquipmentPurchaseType> GetPagedEquipmentPurchaseTypes(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._equipmentPurchaseTypeRepository.Table.OrderBy(s => s.Name);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public EquipmentPurchaseType GetEquipmentPurchaseTypeById(int id)
        {
            return this._equipmentPurchaseTypeRepository.GetById(id);
        }

        #endregion
    }
}
