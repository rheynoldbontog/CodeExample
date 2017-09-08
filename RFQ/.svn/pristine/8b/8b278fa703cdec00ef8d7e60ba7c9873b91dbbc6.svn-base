using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class user
    {
        public user()
        {
            this.activitylogs = new List<activitylog>();
            this.externalauthenticationrecords = new List<externalauthenticationrecord>();
            this.forumsposts = new List<forumspost>();
            this.forumsprivatemessages = new List<forumsprivatemessage>();
            this.forumsprivatemessages1 = new List<forumsprivatemessage>();
            this.forumssubscriptions = new List<forumssubscription>();
            this.forumstopics = new List<forumstopic>();
            this.logs = new List<log>();
            this.usercontents = new List<usercontent>();
            this.addresses = new List<address>();
            this.userroles = new List<userrole>();
        }

        public int Id { get; set; }
        public System.Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PasswordFormatId { get; set; }
        public string PasswordSalt { get; set; }
        public string AdminComment { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<int> LanguageId { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public string TimeZoneId { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool IsSystemAccount { get; set; }
        public string SystemName { get; set; }
        public string LastIpAddress { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
        public Nullable<System.DateTime> LastLoginDateUtc { get; set; }
        public System.DateTime LastActivityDateUtc { get; set; }
        public Nullable<int> BillingAddress_Id { get; set; }
        public Nullable<int> ShippingAddress_Id { get; set; }
        public virtual ICollection<activitylog> activitylogs { get; set; }
        public virtual address address { get; set; }
        public virtual address address1 { get; set; }
        public virtual currency currency { get; set; }
        public virtual ICollection<externalauthenticationrecord> externalauthenticationrecords { get; set; }
        public virtual ICollection<forumspost> forumsposts { get; set; }
        public virtual ICollection<forumsprivatemessage> forumsprivatemessages { get; set; }
        public virtual ICollection<forumsprivatemessage> forumsprivatemessages1 { get; set; }
        public virtual ICollection<forumssubscription> forumssubscriptions { get; set; }
        public virtual ICollection<forumstopic> forumstopics { get; set; }
        public virtual language language { get; set; }
        public virtual ICollection<log> logs { get; set; }
        public virtual ICollection<usercontent> usercontents { get; set; }
        public virtual ICollection<address> addresses { get; set; }
        public virtual ICollection<userrole> userroles { get; set; }
    }
}
