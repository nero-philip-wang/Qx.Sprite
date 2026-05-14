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

    /// <summary>
    /// 角色实体类，定义系统中的角色信息
    /// </summary>
    public class Role : FullAuditedEntity<int>, IMultiTenant<string>, IOnModelCreating
    {
        /// <summary>
        /// Gets or sets 应用ID，标识角色所属的应用程序
        /// </summary>
        public string AppId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the tenant associated with the current context.
        /// </summary>
        public string? TenantId { get; set; }

        /// <inheritdoc/>
        object? IMultiTenant.TenantId { get => this.TenantId; set => this.TenantId = (string?)value; }

        /// <summary>
        /// Gets or sets 角色标题/名称
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets 角色关联的权限列表
        /// </summary>
        public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        /// <summary>
        /// Gets or sets 角色关联列表
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        /// <inheritdoc/>
        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(b =>
            {
                b.HasMany(e => e.Users)
                .WithMany(e => e.Roles);
            });
        }
    }
}