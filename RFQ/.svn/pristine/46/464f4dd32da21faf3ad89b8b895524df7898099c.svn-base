using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class news
    {
        public news()
        {
            this.newscomments = new List<newscomment>();
        }

        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string Short { get; set; }
        public string Full { get; set; }
        public bool Published { get; set; }
        public Nullable<System.DateTime> StartDateUtc { get; set; }
        public Nullable<System.DateTime> EndDateUtc { get; set; }
        public bool AllowComments { get; set; }
        public int ApprovedCommentCount { get; set; }
        public int NotApprovedCommentCount { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public virtual language language { get; set; }
        public virtual ICollection<newscomment> newscomments { get; set; }
    }
}
