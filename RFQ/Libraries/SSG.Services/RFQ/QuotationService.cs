using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using SSG.Core.Data;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public class QuotationService : IQuotationService
    {
        #region Fields

        private readonly IRepository<Quotation> _quotationRepository;

        #endregion

        #region ctor

        public QuotationService(IRepository<Quotation> quotationRepository)
        {
            this._quotationRepository = quotationRepository;
        }

        #endregion

        #region Methods
        public IEnumerable<Quotation> GetAllQuotations(bool activeOnly = false)
        {
            return this._quotationRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsEnumerable();
        }

        public IQueryable<Quotation> GetAllQuotationsQueryable(bool activeOnly = false)
        {
            return this._quotationRepository.Table
                .Where(s => (activeOnly ? s.Active : 1 == 1))
                .AsQueryable();
        }

        public void SaveQuotations(Quotation uom)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    if (uom.Id == 0)
                    {
                        this._quotationRepository.Insert(uom);
                    }
                    else
                    {
                        this._quotationRepository.Update(uom);
                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IQueryable<Quotation> GetPagedQuotation(int currentPage, int pageSize, out int totalCount)
        {
            var query = this._quotationRepository.Table.OrderBy(s => s.QuotationNo);

            totalCount = query.Count();

            var pagedList = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            return pagedList;
        }

        public Quotation GetQuotationById(int id)
        {
            return this._quotationRepository.GetById(id);
        }               
        #endregion
    }
}
