using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class forumsprivatemessage
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeletedByAuthor { get; set; }
        public bool IsDeletedByRecipient { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}
