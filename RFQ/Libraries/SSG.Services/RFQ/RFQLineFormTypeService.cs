using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class RFQLineFormTypeService : IRFQLineFormTypeService
    {
        #region Fields

        private readonly IRepository<RFQLineFormType> _rfqLineFormTypeRepository;

        #endregion

        #region ctor

        public RFQLineFormTypeService(IRepository<RFQLineFormType> rfqLineFormTypeRepository)
        {
            this._rfqLineFormTypeRepository = rfqLineFormTypeRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<RFQLineFormType> GetAllRFQLineFormTypes(bool activeOnly = false)
        {
            return this._rfqLineFormTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<RFQLineFormType> GetAllRFQLineFormTypesQueryable(bool activeOnly = false)
        {
            return this._rfqLineFormTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveRFQLineFormType(RFQLineFormType rfqLineFormType)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (rfqLineFormType.Id == 0)
                    {
                        this._rfqLineFormTypeRepository.Insert(rfqLineFormType);
                    }
                    else
                    {
                        this._rfqLineFormTypeRepository.Update(rfqLineFormType);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<RFQLineFormType> GetPagedRFQLineFormTypes(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._rfqLineFormTypeRepository.Table.OrderBy(s => s.Name);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public RFQLineFormType GetRFQLineFormTypeById(int id)
        {
            return this._rfqLineFormTypeRepository.GetById(id);
        }

        public RFQLineFormType GetRFQLineFormTypeByName(string name)
        {
            return this._rfqLineFormTypeRepository.Table.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(name.Trim().ToUpper()));
        }

        #endregion
    }
}
