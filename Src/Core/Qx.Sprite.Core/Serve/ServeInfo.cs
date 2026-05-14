// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <inheritdoc/>
    public class ServeInfo : IServeInfo
    {
    /// <inheritdoc/>
        public string Title { get; set; } = string.Empty;

    /// <inheritdoc/>
        public Version Version { get; set; } = new Version(1, 0, 0);

    /// <inheritdoc/>
        public string ServeBaseUrl { get; set; } = string.Empty;
    }
}