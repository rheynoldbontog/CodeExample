﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IReportService
    {
        bool GeneratePdfReport(SSG.Core.Domain.RFQ.RFQ rfq);
        void createExcel(string filePath,List<RFQLine> rfqLines);
        void ExportQuotation(string filePath, RFQLine rfqLines);
        void ExportAllRFQIndex(string filePath, List<RFQLineService.TableJoin> rfqTableJoins);
    }
}