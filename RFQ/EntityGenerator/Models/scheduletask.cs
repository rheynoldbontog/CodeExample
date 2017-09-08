using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class scheduletask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Seconds { get; set; }
        public string Type { get; set; }
        public bool Enabled { get; set; }
        public bool StopOnError { get; set; }
        public Nullable<System.DateTime> LastStartUtc { get; set; }
        public Nullable<System.DateTime> LastEndUtc { get; set; }
        public Nullable<System.DateTime> LastSuccessUtc { get; set; }
    }
}
