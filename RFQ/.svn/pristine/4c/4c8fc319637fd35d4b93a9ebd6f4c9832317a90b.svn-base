using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class language
    {
        public language()
        {
            this.localestringresources = new List<localestringresource>();
            this.localizedproperties = new List<localizedproperty>();
            this.news = new List<news>();
            this.polls = new List<poll>();
            this.users = new List<user>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCulture { get; set; }
        public string UniqueSeoCode { get; set; }
        public string FlagImageFileName { get; set; }
        public bool Rtl { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<localestringresource> localestringresources { get; set; }
        public virtual ICollection<localizedproperty> localizedproperties { get; set; }
        public virtual ICollection<news> news { get; set; }
        public virtual ICollection<poll> polls { get; set; }
        public virtual ICollection<user> users { get; set; }
    }
}
