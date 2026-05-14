// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 用户角色关联实体类，定义用户与角色的关联关系
    /// </summary>
    public class User : FullAuditedEntity<long>, IMultiTenant<string>, IOnModelCreating
    {
        /// <summary>
        /// Gets or sets the unique identifier of the tenant associated with the current context.
        /// </summary>
        public string? TenantId { get; set; }

        /// <inheritdoc/>
        object? IMultiTenant.TenantId { get => this.TenantId; set => this.TenantId = (string?)value; }

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
        /// Gets or sets 角色ID数组，存储用户拥有的所有角色标识
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

        /// <inheritdoc/>
        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(e => e.Roles)
                .WithMany(e => e.Users);
            });
        }
    }
}