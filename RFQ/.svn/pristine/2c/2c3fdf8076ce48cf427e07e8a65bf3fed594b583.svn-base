using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class forumsgroup
    {
        public forumsgroup()
        {
            this.forumsforums = new List<forumsforum>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
        public virtual ICollection<forumsforum> forumsforums { get; set; }
    }
}
