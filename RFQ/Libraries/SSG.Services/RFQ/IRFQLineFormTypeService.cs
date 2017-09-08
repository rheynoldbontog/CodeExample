using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IRFQLineFormTypeService
    {
        IEnumerable<RFQLineFormType> GetAllRFQLineFormTypes(bool activeOnly = false);
        IQueryable<RFQLineFormType> GetAllRFQLineFormTypesQueryable(bool activeOnly = false);
        void SaveRFQLineFormType(RFQLineFormType rfqLineFormType);
        IQueryable<RFQLineFormType> GetPagedRFQLineFormTypes(int currentPage, int pageSize, out int totalCount);
        RFQLineFormType GetRFQLineFormTypeById(int id);
        RFQLineFormType GetRFQLineFormTypeByName(string name);
    }
}
