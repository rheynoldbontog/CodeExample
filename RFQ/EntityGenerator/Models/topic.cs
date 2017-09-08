using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class topic
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public bool IncludeInSitemap { get; set; }
        public bool IsPasswordProtected { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
    }
}
