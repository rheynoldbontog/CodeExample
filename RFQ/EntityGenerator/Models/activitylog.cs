using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class activitylog
    {
        public int Id { get; set; }
        public int ActivityLogTypeId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public virtual activitylogtype activitylogtype { get; set; }
        public virtual user user { get; set; }
    }
}
