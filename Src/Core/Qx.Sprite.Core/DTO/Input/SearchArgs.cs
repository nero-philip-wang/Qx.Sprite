// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 搜索参数
    /// </summary>
    public class SearchArgs : SortingArgs
    {
        /// <summary>
        /// Gets or sets 通用过滤条件
        /// </summary>
        public string? Filter { get; set; }

        /// <summary>
        /// Gets or sets 过滤参数
        /// </summary>
        public string[] FilterArgs { get; set; } = [];
    }
}