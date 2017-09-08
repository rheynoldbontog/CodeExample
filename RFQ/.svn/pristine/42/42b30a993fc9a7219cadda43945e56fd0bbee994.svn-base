using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IQuotationService
    {
        IEnumerable<Quotation> GetAllQuotations(bool activeOnly = false);
        IQueryable<Quotation> GetAllQuotationsQueryable(bool activeOnly = false);
        void SaveQuotations(Quotation uom);
        IQueryable<Quotation> GetPagedQuotation(int currentPage, int pageSize, out int totalCount);
        Quotation GetQuotationById(int id);
    }
}
