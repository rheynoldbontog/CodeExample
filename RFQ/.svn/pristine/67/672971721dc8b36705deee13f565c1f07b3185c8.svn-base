using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using System.Transactions;
using SSG.Core.EntityExtensions;

namespace SSG.Services.RFQ
{
    public class RFQLineStatusHistoryService : IRFQLineStatusHistoryService
    {
        #region Fields

        private readonly IRepository<RFQLineStatusHistory> _rfqLineStatusHistoryRepository;

        #endregion

        #region ctor

        public RFQLineStatusHistoryService(IRepository<RFQLineStatusHistory> rfqLineStatusHistoryRepository)
        {
            this._rfqLineStatusHistoryRepository = rfqLineStatusHistoryRepository;
        }

        #endregion

        #region Methods

        public IEnumerable<RFQLineStatusHistory> GetAllStatusHistories(bool activeOnly = false)
        {
            return this._rfqLineStatusHistoryRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<RFQLineStatusHistory> GetAllStatusHistoriesQueryable(bool activeOnly = false)
        {
            return this._rfqLineStatusHistoryRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveStatusHistory(int rfqLineId, int oldStatus, int newStatus, int userId)
        {
            var statusHistory = new RFQLineStatusHistory()
            {
                RFQLineId = rfqLineId,
                OldStatusId = oldStatus,
                NewStatusId = newStatus,
                Active = true
            };

            statusHistory.PopulateAuditFieldsForSave(userId);
            SaveStatusHistory(statusHistory);
        }

        public void SaveStatusHistory(RFQLineStatusHistory rfqStatusHistory)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (rfqStatusHistory.Id == 0)
                    {
                        this._rfqLineStatusHistoryRepository.Insert(rfqStatusHistory);
                    }
                    else
                    {
                        this._rfqLineStatusHistoryRepository.Update(rfqStatusHistory);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<RFQLineStatusHistory> GetPagedRfqStatusHistories(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._rfqLineStatusHistoryRepository.Table.OrderBy(s => s.RFQLineId);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public RFQLineStatusHistory GetRfqStatusHistoryById(int id)
        {
            return this._rfqLineStatusHistoryRepository.GetById(id);
        }

        #endregion
    }
}
