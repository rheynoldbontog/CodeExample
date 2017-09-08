using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SSG.Web.Models.RFQ
{
    public class FileAttachmentModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Filename")]
        public string Filename { get; set; }

        [DisplayName("FileUrl")]
        public string FileUrl { get; set; }

        [DisplayName("File Attachment Type")]
        public int FileAttachmentType { get; set; }

        [Required]
        [DisplayName("Active")]
        public bool Active { get; set; }

        public int? RFQLineId { get; set; }

        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime DateCreatedOnUtc { get; set; }
        public DateTime DateUpdatedOnUtc { get; set; }

        public string QuotationIndex { get; set; }

        public string Index { get; set; }

        public bool IsDeleted { get; set; }

        public bool RFQIsSubmitted { get; set; }
    }
}