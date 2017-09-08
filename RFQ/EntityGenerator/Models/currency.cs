using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class currency
    {
        public currency()
        {
            this.users = new List<user>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public string DisplayLocale { get; set; }
        public string CustomFormatting { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public System.DateTime UpdatedOnUtc { get; set; }
        public virtual ICollection<user> users { get; set; }
    }
}
