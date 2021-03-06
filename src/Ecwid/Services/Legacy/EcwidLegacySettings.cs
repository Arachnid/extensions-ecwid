﻿// Licensed under the GPL License, Version 3.0. See LICENSE in the git repository root for license information.

namespace Ecwid.Legacy
{
    /// <summary>
    /// Settings for Ecwid Legacy Client <see cref="EcwidLegacyClient" />
    /// </summary>
    public class EcwidLegacySettings : EcwidSettings
    {
        /// <summary>
        /// Gets or sets the API URL. Default is https://app.ecwid.com/api/v1/.
        /// </summary>
        /// <value>
        /// The API URL.
        /// </value>
        public override string ApiUrl { get; set; } = "https://app.ecwid.com/api/v1/";

        /// <summary>
        /// Gets or sets the maximum seconds to wait lock limit. From 1. Default is 600 sec. = 10 min.
        /// </summary>
        /// <value>
        /// The maximum seconds to wait lock limit.
        /// </value>
        public int MaxSecondsToWait { get; set; } = 600;

        /// <summary>
        /// Gets or sets the retry interval to ask for lock limit in sec. From 1. Default is 1 sec.
        /// </summary>
        /// <value>
        /// The retry interval in sec.
        /// </value>
        public int RetryInterval { get; set; } = 1;
    }
}