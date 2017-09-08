﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IRFQLineService
    {
        IEnumerable<RFQLine> GetAllRFQLines(bool activeOnly = false);
        IQueryable<RFQLine> GetAllRFQLinesQueryable(bool activeOnly = false);
        void SaveRFQLine(RFQLine line);
        IQueryable<RFQLine> GetPagedRFQLines(int currentPage, int pageSize, out int totalCount);
        RFQLine GetRFQLineById(int id);
        IQueryable<RFQLineService.TableJoin> GetAllQueryableTableJoins();
    }
}