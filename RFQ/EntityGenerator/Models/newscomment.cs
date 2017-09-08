using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class newscomment
    {
        public int Id { get; set; }
        public string CommentTitle { get; set; }
        public string CommentText { get; set; }
        public int NewsItemId { get; set; }
        public virtual news news { get; set; }
        public virtual usercontent usercontent { get; set; }
    }
}
