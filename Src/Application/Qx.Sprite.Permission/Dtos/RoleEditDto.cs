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
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;

    /// <summary>
    /// 角色编辑数据传输对象
    /// </summary>
    public class RoleEditDto
    {
        /// <summary>
        /// Gets or sets 应用ID，标识角色所属的应用程序
        /// </summary>
        public string AppId { get; set; } = null!;

        /// <summary>
        /// Gets or sets 租户ID
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets or sets 角色标题/名称
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets 角色关联的权限列表
        /// </summary>
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();

        /// <summary>
        /// Gets or sets 角色关联列表
        /// </summary>
        public virtual List<long> UserIds { get; set; } = new List<long>();
    }
}