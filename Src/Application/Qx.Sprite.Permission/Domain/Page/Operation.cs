// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using Qx.Sprite.Domain;

    /// <summary>
    /// 页面操作
    /// </summary>
    public class Operation : Entity<int>
    {
        /// <summary>
        /// Gets or sets 关联页面ID
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Gets or sets 操作编码
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Gets or sets 操作标题
        /// </summary>
        public string Title { get; set; } = null!;
    }
}