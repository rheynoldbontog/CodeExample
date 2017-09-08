using SSG.Core.Configuration;

namespace SSG.Core.Domain.Messages
{
    public class EmailAccountSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a site default email account identifier
        /// </summary>
        public int DefaultEmailAccountId { get; set; }

    }

}
