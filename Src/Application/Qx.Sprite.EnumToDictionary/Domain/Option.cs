// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionary
{
    /// <summary>
    /// 选项
    /// </summary>
    public class Option : IDictionary
    {
        /// <inheritdoc/>
        public string Type { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Key { get; set; } = string.Empty;

        /// <inheritdoc/>
        public string Label { get; set; } = string.Empty;

        /// <inheritdoc/>
        public int Value { get; set; }

        /// <inheritdoc/>
        public bool Enabled { get; set; } = true;

        /// <inheritdoc/>
        public bool IsEnum { get; set; } = true;
    }
}