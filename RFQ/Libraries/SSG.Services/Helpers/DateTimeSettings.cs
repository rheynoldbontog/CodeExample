﻿
using SSG.Core.Configuration;

namespace SSG.Services.Helpers
{
    public class DateTimeSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a default site time zone identifier
        /// </summary>
        public string DefaultSiteTimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users are allowed to select theirs time zone
        /// </summary>
        public bool AllowUsersToSetTimeZone { get; set; }
    }
}