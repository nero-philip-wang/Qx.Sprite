// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 角色权限
    /// </summary>
    public class Permission : Entity<int>
    {
        /// <summary>
        /// Gets or sets 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets 角色
        /// </summary>
        public virtual Role Role { get; set; } = null!;

        /// <summary>
        /// Gets or sets 页面ID
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Gets or sets 页面
        /// </summary>
        public virtual Page Page { get; set; } = null!;

        /// <summary>
        /// Gets or sets 操作权限
        /// </summary>
        [JsonField]
        public int[] ActionPermission { get; set; } = Array.Empty<int>();

        /// <summary>
        /// Gets or sets 接入点权限
        /// </summary>
        [JsonField]
        public int[] EndPoints { get; set; } = Array.Empty<int>();
    }
}