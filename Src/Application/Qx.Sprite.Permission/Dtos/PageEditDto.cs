// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using Qx.Sprite.EFCore;
    using static Qx.Sprite.Permission.Page;

    /// <summary>
    /// 页面
    /// </summary>
    public class PageEditDto
    {
        /// <summary>
        /// Gets or sets 租户ID
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets or sets 页面归属于哪个客户端
        /// </summary>
        public string AppId { get; set; } = null!;

        /// <summary>
        /// Gets or sets 页面名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets 页面路径
        /// </summary>
        public string Path { get; set; } = null!;

        /// <summary>
        /// Gets or sets 页面重定向路径
        /// </summary>
        public string Redirect { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 页面元数据
        /// </summary>
        public PageMeta Meta { get; set; } = new PageMeta();

        /// <summary>
        /// Gets or sets 父页面ID
        /// </summary>
        public int? ParentId { get; set; }
    }
}