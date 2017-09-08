using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SSG.Web.Models.RFQ
{
    public class DepartmentModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Active")]
        public bool Active { get; set; }

        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime DateCreatedOnUtc { get; set; }
        public DateTime DateUpdatedOnUtc { get; set; }
    }
}