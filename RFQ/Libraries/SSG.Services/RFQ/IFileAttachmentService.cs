﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.RFQ;

namespace SSG.Services.RFQ
{
    public interface IFileAttachmentService
    {
        IEnumerable<FileAttachment> GetAllFileAttachmentsEnumerableForRFQLine(int rfqLineId, bool activeOnly = false);
        IQueryable<FileAttachment> GetAllFileAttachmentsQueryableForRFQLine(int rfqLineId, bool activeOnly = false);
        FileAttachment GetFileAttachmentById(int id);
        void SaveFileAttachment(FileAttachment attachment);
    }
}
