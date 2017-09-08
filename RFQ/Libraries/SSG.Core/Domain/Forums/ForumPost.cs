﻿using System;
using SSG.Core.Domain.Users;

namespace SSG.Core.Domain.Forums
{
    /// <summary>
    /// Represents a forum post
    /// </summary>
    public partial class ForumPost : BaseEntity
    {
        /// <summary>
        /// Gets or sets the forum topic identifier
        /// </summary>
        public virtual int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// Gets or sets the text
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the IP address
        /// </summary>
        public virtual string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public virtual DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance update
        /// </summary>
        public virtual DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets the topic
        /// </summary>
        public virtual ForumTopic ForumTopic { get; set; }

        /// <summary>
        /// Gets the user
        /// </summary>
        public virtual User User { get; set; }

    }
}
