// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 权限数据库上下文包
    /// </summary>
    /// <remarks>
    /// 初始化权限数据库上下文包
    /// </remarks>
    /// <param name="configuration">配置</param>
    /// <param name="env">环境</param>
    [TodoPack]
    public abstract class PermissionDbContextPack(IConfiguration configuration, IHostEnvironment env) : AppDbContextPack<PermissionDbContext>(configuration, env)
    {
        /// <inheritdoc/>
        protected override DbContextOptionsBuilder UseServe(DbContextOptionsBuilder builder)
        {
            var conn = this.configuration.GetConnectionString("Auth") ?? this.configuration.GetConnectionString("Default");
            conn.CheckNotNull("Permission Connection String should not be null.");
            return this.UseDb(builder, conn);
        }

        /// <summary>
        /// 配置数据库
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected abstract DbContextOptionsBuilder UseDb(DbContextOptionsBuilder builder, string connectionString);
    }
}