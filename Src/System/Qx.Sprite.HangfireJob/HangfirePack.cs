// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.BackgroundJob
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using Hangfire;
    using Hangfire.Dashboard;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Builder.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;

    /// <summary>
    /// Hangfire 后台任务模块
    /// </summary>
    public class HangfirePack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HangfirePack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public HangfirePack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <summary>
        /// 配置 hangfire
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="configuration"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public delegate IGlobalConfiguration ConfigStorageDelegate(HangfirePack instance, IGlobalConfiguration configuration, string connectionString);

        /// <summary>
        /// Gets or sets the delegate for configuring the database.
        /// </summary>
        public static ConfigStorageDelegate? ConfigStorage { get; set; }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override int Order => 10;

        /// <summary>
        /// Gets or sets hangfire Dashboard 授权过滤器列表，默认为匿名访问
        /// </summary>
        public IEnumerable<IDashboardAuthorizationFilter> Authorization { get; set; } = [new AnonymousAuthorizeFilter()];

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            var sql = this.configuration.GetConnectionString("Default") ?? this.configuration["AspNetPack:Hangfire:Conn"];
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new Exception("配置文件中连接字符串或者AspNetPack:Hangfire:Conn节点不能同时为空");
            }

            // Add Hangfire services.
            services.AddHangfire(conf =>
            {
                conf = conf.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings();
                conf = this.UseConfig(conf, sql);
            });

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseHangfireDashboard(options: new DashboardOptions() { Authorization = this.Authorization });
            var service = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ISheduledTaskService>();
            service.Init();
            base.UsePack(app);
        }

        /// <summary>
        /// 配置数据库
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public virtual IGlobalConfiguration UseConfig(IGlobalConfiguration configuration, string connectionString)
        {
            if (ConfigStorage != null)
            {
                return ConfigStorage(this, configuration, connectionString);
            }
            else
            {
                throw new NotImplementedException("请设置 ConfigStorage 委托");
            }
        }
    }
}