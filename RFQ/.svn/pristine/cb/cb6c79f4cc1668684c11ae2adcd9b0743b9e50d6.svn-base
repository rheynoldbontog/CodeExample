using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.PSP;

namespace SSG.Services.PSP
{
    public interface ISupportCodeService
    {
        IEnumerable<SupportCode> GetAllSupportCodes(bool activeOnly = false);
        IQueryable<SupportCode> GetAllSupportCodeQueryable(bool activeOnly = false);
        void SaveSupportCode(SupportCode supportCode);
        IQueryable<SupportCode> GetPagedSupportCodes(int currentPage, int pageSize, out int totalCount);
        SupportCode GetSupportCodeById(int id);
    }
}
