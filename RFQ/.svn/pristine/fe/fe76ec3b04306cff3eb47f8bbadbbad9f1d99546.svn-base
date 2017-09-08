using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class stateprovince
    {
        public stateprovince()
        {
            this.addresses = new List<address>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<address> addresses { get; set; }
        public virtual country country { get; set; }
    }
}
