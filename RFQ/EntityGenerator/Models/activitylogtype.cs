using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class activitylogtype
    {
        public activitylogtype()
        {
            this.activitylogs = new List<activitylog>();
        }

        public int Id { get; set; }
        public string SystemKeyword { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public virtual ICollection<activitylog> activitylogs { get; set; }
    }
}
