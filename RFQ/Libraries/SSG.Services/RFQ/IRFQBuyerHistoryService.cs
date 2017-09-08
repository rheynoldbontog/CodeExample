﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IRFQBuyerHistoryService
    {
        void SaveBuyerHistory(int rfqId, int oldBuyerId, int newBuyerId, string changeReason, int userId);
        void SaveBuyerHistory(RFQBuyerHistory rfqBuyerHistory);
    }
}