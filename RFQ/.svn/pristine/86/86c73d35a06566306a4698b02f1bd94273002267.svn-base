using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IRFQLineTypeService
    {
        IEnumerable<RFQLineType> GetAllRFQLineTypes(bool activeOnly = false);
        IQueryable<RFQLineType> GetAllRFQLineTypesQueryable(bool activeOnly = false);
        void SaveRFQLineType(RFQLineType rfqLineType);
        IQueryable<RFQLineType> GetPagedRFQLineTypes(int currentPage, int pageSize, out int totalCount);
        RFQLineType GetRFQLineTypeById(int id);
        RFQLineType GetRFQLineTypeByName(string name);
    }
}
