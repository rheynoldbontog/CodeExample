using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class forumstopic
    {
        public forumstopic()
        {
            this.forumsposts = new List<forumspost>();
        }

        public int Id { get; set; }
        public int ForumId { get; set; }
        public int UserId { get; set; }
        public int TopicTypeId { get; set; }
        public string Subject { get; set; }
        public int NumPosts { get; set; }
        public int Views { get; set; }
        public int LastPostId { get; set; }
        public int LastPostUserId { get; set; }
        public Nullable<System.DateTime> LastPostTime { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
        public virtual forumsforum forumsforum { get; set; }
        public virtual ICollection<forumspost> forumsposts { get; set; }
        public virtual user user { get; set; }
    }
}
