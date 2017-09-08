using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class address
    {
        public address()
        {
            this.users = new List<user>();
            this.users1 = new List<user>();
            this.users2 = new List<user>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> StateProvinceId { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipPostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public virtual country country { get; set; }
        public virtual stateprovince stateprovince { get; set; }
        public virtual ICollection<user> users { get; set; }
        public virtual ICollection<user> users1 { get; set; }
        public virtual ICollection<user> users2 { get; set; }
    }
}
