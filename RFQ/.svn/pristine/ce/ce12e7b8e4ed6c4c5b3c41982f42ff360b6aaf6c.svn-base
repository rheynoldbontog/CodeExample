using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;
using SSG.Core.EntityExtensions;
using System.Transactions;

namespace SSG.Services.RFQ
{
    public class RFQBuyerHistoryService : IRFQBuyerHistoryService
    {
        #region Fields

        private readonly IRepository<RFQBuyerHistory> _rfqBuyerHistoryRepository;

        #endregion

        #region ctor

        public RFQBuyerHistoryService(IRepository<RFQBuyerHistory> rfqBuyerHistoryRepository)
        {
            this._rfqBuyerHistoryRepository = rfqBuyerHistoryRepository;
        }

        #endregion

        #region Methods

        public void SaveBuyerHistory(int rfqId, int oldBuyerId, int newBuyerId, string changeReason, int userId)
        {
            var buyerHistory = new RFQBuyerHistory()
            {
                RFQId = rfqId,
                OldBuyerId = oldBuyerId,
                NewBuyerId = newBuyerId,
                ChangeReason = changeReason,
                Active = true
            };

            buyerHistory.PopulateAuditFieldsForSave(userId);

            SaveBuyerHistory(buyerHistory);
        }

        public void SaveBuyerHistory(RFQBuyerHistory rfqBuyerHistory)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (rfqBuyerHistory.Id == 0)
                    {
                        this._rfqBuyerHistoryRepository.Insert(rfqBuyerHistory);
                    }
                    else
                    {
                        this._rfqBuyerHistoryRepository.Update(rfqBuyerHistory);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #endregion
    }
}
