// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission.Controller
{
    /// <summary>
    /// 后端接入点
    /// </summary>
    public class EndPointEditDto
    {
        /// <summary>
        /// Gets or sets 关联页面ID
        /// </summary>
        public int? PageId { get; set; }

        /// <summary>
        /// Gets or sets 接入点命名空间
        /// </summary>
        public string Namespace { get; set; } = null!;

        /// <summary>
        /// Gets or sets 接入点控制器
        /// </summary>
        public string Controller { get; set; } = null!;

        /// <summary>
        /// Gets or sets 接入点操作
        /// </summary>
        public string Action { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets 接入点标题
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}