using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;


namespace SSG.Services.RFQ
{
    public interface IRFQLineStatusService
    {
        IEnumerable<RFQLineStatus> GetAllStatus(bool activeOnly = false);
        IQueryable<RFQLineStatus> GetAllQueryableStatus(bool activeOnly = false);
        void SaveRfqLineStatus(RFQLineStatus rfqLineStatus);
        RFQLineStatus GetRfqLineStatusById(int id);
    }
}
