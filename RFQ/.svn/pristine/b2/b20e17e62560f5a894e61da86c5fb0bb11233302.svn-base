using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SSG.Web.Models.PSP
{
    public class SupportCodeModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Support Code")]
        public string SupportCodes { get; set; }

        [Required]
        [DisplayName("Support Code Description")]
        public string SupportCodeDescription { get; set; }

        [Required]
        [DisplayName("Active")]
        public bool Active { get; set; }

        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime DateCreatedOnUtc { get; set; }
        public DateTime DateUpdatedOnUtc { get; set; }

    }
}