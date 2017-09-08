using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class RFQStatusService : IRFQStatusService
    {
        #region Fields

        private readonly IRepository<RFQStatus> _rFQStatusRepository;

        #endregion

        #region ctor

        public RFQStatusService(IRepository<RFQStatus> rFQStatusRepository)
        {
            this._rFQStatusRepository = rFQStatusRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<RFQStatus> GetAllRFQStatus(bool activeOnly = false)
        {
            return this._rFQStatusRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<RFQStatus> GetAllRFQStatusQueryable(bool activeOnly = false)
        {
            return this._rFQStatusRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveRFQStatus(RFQStatus rFQStatus)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (rFQStatus.Id == 0)
                    {
                        this._rFQStatusRepository.Insert(rFQStatus);
                    }
                    else
                    {
                        this._rFQStatusRepository.Update(rFQStatus);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<RFQStatus> GetPagedRFQStatus(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._rFQStatusRepository.Table.OrderBy(s => s.Name);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public RFQStatus GetRFQStatusById(int id)
        {
            return this._rFQStatusRepository.GetById(id);
        }

        public RFQStatus GetRFQStatusByName(string name)
        {
            return this._rFQStatusRepository.Table.FirstOrDefault(x => x.Name.Equals(name));
        }

        #endregion

    }
}
