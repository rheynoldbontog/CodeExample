using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class newslettersubscription
    {
        public int Id { get; set; }
        public System.Guid NewsLetterSubscriptionGuid { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
    }
}
