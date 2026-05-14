// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    /// <summary>
    /// 页面元数据
    /// </summary>
    public class PageMeta
    {
        /// <summary>
        /// Gets or sets 图标
        /// </summary>
        public string Icon { get; set; } = null!;

        /// <summary>
        /// Gets or sets 标题
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets 权限
        /// </summary>
        public string[] Permission { get; set; } = null!;

        /// <summary>
        /// Gets or sets 激活菜单
        /// </summary>
        public string ActiveMenu { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets 是否不缓存
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets 是否总是显示
        /// </summary>
        public bool AlwaysShow { get; set; }
    }
}