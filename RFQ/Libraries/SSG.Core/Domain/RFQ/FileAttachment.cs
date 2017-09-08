using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSG.Core.Domain.RFQ
{
    public enum FileAttachmentType
    {
        RFQLineAttachment = 1,
        QuoteAttachment = 2,
        ROHSDocumentAttachment = 3,
        MSDSDocumentAttachment = 4,
        OtherAttachmentDrawing = 5
    }

    public partial class FileAttachment : BaseAuditEntity
    {
        public string Filename { get; set; }

        public string FileUrl { get; set; }

        public int FileAttachmentType { get; set; }

        public bool Active { get; set; }

    }
}
