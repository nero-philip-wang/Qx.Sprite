// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Cap
{
    using System;
    using DotNetCore.CAP;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;

    /// <summary>
    /// CAP 服务包
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="env"></param>
    public class CapPack(IConfiguration configuration, IHostEnvironment env) : AspNetPack(configuration, env)
    {
        /// <summary>
        /// 配置 CAP
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public delegate CapOptions ConfigCapDelegate(CapPack instance, CapOptions options, IConfiguration configuration);

        /// <summary>
        /// Gets or sets the delegate for configuring the database.
        /// </summary>
        public static ConfigCapDelegate? ConfigCap { get; set; }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddCap(x =>
            {
                x.UseDashboard();

                this.UseConfig(x, this.configuration);
            }).AddSubscribeFilter<CapHeaderFilter>();

            return base.AddServices(services);
        }

        /// <summary>
        /// 配置消息队列和数据库
        /// </summary>
        /// <param name="x"></param>
        /// <param name="configuration"></param>
        /// <exception cref="NotImplementedException">请设置 ConfigCap 委托</exception>
        protected virtual void UseConfig(CapOptions x, IConfiguration configuration)
        {
            if (ConfigCap != null)
            {
                ConfigCap(this, x, configuration);
            }
            else
            {
                throw new NotImplementedException("请设置 ConfigCap 委托配置消息队列和数据库");
            }
        }
    }
}