// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission.Controller
{
    /// <summary>
    /// 页面操作
    /// </summary>
    public class OperationEditDto
    {
        /// <summary>
        /// Gets or sets 主键
        /// </summary>
        public int Id { get; set; }

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