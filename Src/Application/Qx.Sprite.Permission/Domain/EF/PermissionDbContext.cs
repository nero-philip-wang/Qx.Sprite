// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 权限数据库上下文
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PermissionDbContext"/> class.
    /// </remarks>
    /// <param name="options"></param>
    public partial class PermissionDbContext(DbContextOptions<PermissionDbContext> options) : AppDbContext(options)
    {
        /// <summary>
        /// Gets or sets 页面
        /// </summary>
        public DbSet<Page> Pages { get; set; }

        /// <summary>
        /// Gets or sets 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets 权限
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// Gets or sets 用户角色
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets 后端接入点
        /// </summary>
        public DbSet<EndPoint> EndPoints { get; set; }

        /// <summary>
        /// Gets or sets 操作
        /// </summary>
        public DbSet<Operation> Operations { get; set; }

        /// <inheritdoc/>
        protected override string Schema => "dbo";

        /// <inheritdoc/>
        protected override string TableNamePrefix => "auth";
    }
}