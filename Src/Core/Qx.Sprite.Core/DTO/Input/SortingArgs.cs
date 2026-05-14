// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 排序参数
    /// </summary>
    public class SortingArgs : PagingArgs
    {
        /// <summary>
        /// Gets or sets 排序字段，默认按id降序排列
        /// </summary>
        public string OrderBy { get; set; } = "id desc";
    }
}