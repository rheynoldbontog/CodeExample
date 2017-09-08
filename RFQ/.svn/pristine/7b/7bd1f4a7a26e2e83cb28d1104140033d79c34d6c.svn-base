using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class forumspost
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public string IPAddress { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
        public virtual forumstopic forumstopic { get; set; }
        public virtual user user { get; set; }
    }
}
