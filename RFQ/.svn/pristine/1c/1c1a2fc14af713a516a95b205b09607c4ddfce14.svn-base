using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IUOMService
    {
        IEnumerable<UOM> GetAllUOMs(bool activeOnly = false);
        IQueryable<UOM> GetAllUOMsQueryable(bool activeOnly = false);
        void SaveUOM(UOM uom);
        IQueryable<UOM> GetPagedUOMs(int currentPage, int pageSize, out int totalCount);
        UOM GetUOMById(int id);
    }
}
