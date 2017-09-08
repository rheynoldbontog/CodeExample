
using System;

namespace SSG.Core.Domain.Users
{
    /// <summary>
    /// Represents a user generated content
    /// </summary>
    public partial class UserContent : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public virtual string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is approved
        /// </summary>
        public virtual bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public virtual DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public virtual DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public virtual User User { get; set; }
    }
}
