// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 用户角色编辑数据传输对象
    /// </summary>
    public class UserEditDto
    {
        /// <summary>
        /// Gets or sets 租户ID
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Gets or sets 用户ID，关联的用户标识
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Gets or sets 用户手机号，用户的联系信息
        /// </summary>
        public string Mobile { get; set; } = null!;

        /// <summary>
        /// Gets or sets 用户姓名，用户的显示名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets 角色ID列表，存储用户拥有的所有角色标识
        /// </summary>
        public List<int> RoleIds { get; set; } = new List<int>();
    }
}