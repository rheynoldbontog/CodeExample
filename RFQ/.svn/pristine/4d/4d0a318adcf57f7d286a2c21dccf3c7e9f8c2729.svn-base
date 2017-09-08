using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class log
    {
        public int Id { get; set; }
        public int LogLevelId { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public string IpAddress { get; set; }
        public Nullable<int> UserId { get; set; }
        public string PageUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public virtual user user { get; set; }
    }
}
