// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EFCore
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Qx.Sprite.Core;

    /// <summary>
    /// 数据库上下文包
    /// </summary>
    /// <typeparam name="T">数据库上下文类型</typeparam>
    [TodoPack]
    public abstract class AppDbContextPack<T> : AspNetPack
        where T : AppDbContext
    {
        /// <summary>
        /// 是否自动迁移数据库
        /// </summary>
        protected bool isToMigrateDB = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContextPack{T}"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public AppDbContextPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddDbContext<T>(opt =>
            {
                this.UseServe(opt);
            });
            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            base.UsePack(app);
            var scoped = app.ApplicationServices.CreateScope().ServiceProvider;
            var db = scoped.GetService<T>();
            var log = scoped.GetLogger<AppDbContextPack<T>>();
            if (db != null)
            {
                // db.Database.EnsureCreated();
                try
                {
                    if (this.isToMigrateDB)
                        db.Database.Migrate();
                }
                catch (Exception e)
                {
                    log.LogError(e, "数据库迁移失败");
                }
            }
        }

        /// <summary>
        /// 配置数据库上下文
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected abstract DbContextOptionsBuilder UseServe(DbContextOptionsBuilder builder);
    }
}