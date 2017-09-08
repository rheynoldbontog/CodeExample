using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class queuedemail
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string CC { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public int SentTries { get; set; }
        public Nullable<System.DateTime> SentOnUtc { get; set; }
        public int EmailAccountId { get; set; }
        public virtual emailaccount emailaccount { get; set; }
    }
}
