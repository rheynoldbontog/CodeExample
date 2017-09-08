using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class UOMService : IUOMService
    {
        #region Fields

        private readonly IRepository<UOM> _uomRepository;

        #endregion

        #region ctor

        public UOMService(IRepository<UOM> uomRepository)
        {
            this._uomRepository = uomRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<UOM> GetAllUOMs(bool activeOnly = false)
        {
            return this._uomRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<UOM> GetAllUOMsQueryable(bool activeOnly = false)
        {
            return this._uomRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveUOM(UOM uom)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (uom.Id == 0)
                    {
                        this._uomRepository.Insert(uom);
                    }
                    else
                    {
                        this._uomRepository.Update(uom);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<UOM> GetPagedUOMs(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._uomRepository.Table.OrderBy(s => s.Name);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public UOM GetUOMById(int id)
        {
            return this._uomRepository.GetById(id);
        }

        #endregion
    }
}
