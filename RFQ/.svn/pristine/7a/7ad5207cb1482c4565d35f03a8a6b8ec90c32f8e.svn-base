using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;
using SSG.Core.Data;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class RFQLineTypeService : IRFQLineTypeService
    {
        #region Fields

        private readonly IRepository<RFQLineType> _rfqLineTypeRepository;

        #endregion

        #region ctor

        public RFQLineTypeService(IRepository<RFQLineType> rfqLineTypeRepository)
        {
            this._rfqLineTypeRepository = rfqLineTypeRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<RFQLineType> GetAllRFQLineTypes(bool activeOnly = false)
        {
            return this._rfqLineTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<RFQLineType> GetAllRFQLineTypesQueryable(bool activeOnly = false)
        {
            return this._rfqLineTypeRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveRFQLineType(RFQLineType rfqLineType)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (rfqLineType.Id == 0)
                    {
                        this._rfqLineTypeRepository.Insert(rfqLineType);
                    }
                    else
                    {
                        this._rfqLineTypeRepository.Update(rfqLineType);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<RFQLineType> GetPagedRFQLineTypes(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._rfqLineTypeRepository.Table.OrderBy(s => s.Name);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public RFQLineType GetRFQLineTypeById(int id)
        {
            return this._rfqLineTypeRepository.GetById(id);
        }

        public RFQLineType GetRFQLineTypeByName(string name)
        {
            return this._rfqLineTypeRepository.Table.FirstOrDefault(x => x.Name.Trim().ToUpper().Equals(name.Trim().ToUpper()));
        }

        #endregion
    }
}
