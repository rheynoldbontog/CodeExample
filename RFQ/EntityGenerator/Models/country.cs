using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class country
    {
        public country()
        {
            this.addresses = new List<address>();
            this.stateprovinces = new List<stateprovince>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool AllowsBilling { get; set; }
        public bool AllowsShipping { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
        public int NumericIsoCode { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<address> addresses { get; set; }
        public virtual ICollection<stateprovince> stateprovinces { get; set; }
    }
}
