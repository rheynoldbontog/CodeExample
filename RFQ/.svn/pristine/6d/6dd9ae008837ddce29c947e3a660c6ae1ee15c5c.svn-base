using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IRFQStatusService
    {
        IEnumerable<RFQStatus> GetAllRFQStatus(bool activeOnly = false);
        IQueryable<RFQStatus> GetAllRFQStatusQueryable(bool activeOnly = false);
        void SaveRFQStatus(RFQStatus rFQStatus);
        IQueryable<RFQStatus> GetPagedRFQStatus(int currentPage, int pageSize, out int totalCount);
        RFQStatus GetRFQStatusById(int id);
        RFQStatus GetRFQStatusByName(string name);
    }
}
