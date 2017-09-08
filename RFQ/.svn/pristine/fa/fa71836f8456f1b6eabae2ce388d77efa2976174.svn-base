using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class forumsforum
    {
        public forumsforum()
        {
            this.forumstopics = new List<forumstopic>();
        }

        public int Id { get; set; }
        public int ForumGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumTopics { get; set; }
        public int NumPosts { get; set; }
        public int LastTopicId { get; set; }
        public int LastPostId { get; set; }
        public int LastPostUserId { get; set; }
        public Nullable<System.DateTime> LastPostTime { get; set; }
        public int DisplayOrder { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
        public virtual forumsgroup forumsgroup { get; set; }
        public virtual ICollection<forumstopic> forumstopics { get; set; }
    }
}
