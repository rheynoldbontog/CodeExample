using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSG.Core.Domain.Users;

namespace SSG.Core
{
    public abstract partial class BaseAuditEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the date created on UTC.
        /// </summary>
        /// <value>
        /// The date created on UTC.
        /// </value>
        public virtual DateTime DateCreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date updated on UTC.
        /// </summary>
        /// <value>
        /// The date updated on UTC.
        /// </value>
        public virtual DateTime DateUpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the created by user id.
        /// </summary>
        /// <value>
        /// The created by user id.
        /// </value>
        public virtual int CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the updated by user id.
        /// </summary>
        /// <value>
        /// The updated by user id.
        /// </value>
        public virtual int UpdatedByUserId { get; set; }

        #region Who columns

        public virtual User CreatedByUser { get; set; }

        public virtual User UpdatedByUser { get; set; }

        #endregion
    }
}
