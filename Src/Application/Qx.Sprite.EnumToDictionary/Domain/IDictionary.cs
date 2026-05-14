// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionary
{
    using Qx.Sprite.Core;

    /// <summary>
    /// 字典接口
    /// </summary>
    public interface IDictionary
    {
        /// <summary>
        /// Gets or sets 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets 值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets 标签
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 是否枚举
        /// </summary>
        public bool IsEnum { get; set; }
    }
}