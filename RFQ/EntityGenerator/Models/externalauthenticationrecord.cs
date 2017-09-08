using System;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    public partial class externalauthenticationrecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string ExternalIdentifier { get; set; }
        public string ExternalDisplayIdentifier { get; set; }
        public string OAuthToken { get; set; }
        public string OAuthAccessToken { get; set; }
        public string ProviderSystemName { get; set; }
        public virtual user user { get; set; }
    }
}
