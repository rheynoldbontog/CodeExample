using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public class RFQLineStatusService : IRFQLineStatusService
    {
        private readonly IRepository<RFQLineStatus> _rfqLineStatusRepository;

        public RFQLineStatusService(IRepository<RFQLineStatus> rfqLineStatusRepository)
        {
            this._rfqLineStatusRepository = rfqLineStatusRepository;
        }

        public IEnumerable<RFQLineStatus> GetAllStatus(bool activeOnly = false)
        {
            return _rfqLineStatusRepository.Table
                .Where(r => (activeOnly ? r.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<RFQLineStatus> GetAllQueryableStatus(bool activeOnly = false)
        {
            return _rfqLineStatusRepository.Table
                .Where(r => (activeOnly ? r.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveRfqLineStatus(RFQLineStatus rfqLineStatus)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (rfqLineStatus.Id == 0)
                    {
                        _rfqLineStatusRepository.Insert(rfqLineStatus);
                    }
                    else
                    {
                        _rfqLineStatusRepository.Update(rfqLineStatus);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public RFQLineStatus GetRfqLineStatusById(int id)
        {
            return _rfqLineStatusRepository.GetById(id);
        }
    }
}
