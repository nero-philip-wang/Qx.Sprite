// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// 服务包
    /// </summary>
    public abstract class AspNetPack
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected IConfiguration configuration;

        /// <summary>
        /// 环境
        /// </summary>
        protected IHostEnvironment environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public AspNetPack(IConfiguration configuration, IHostEnvironment env)
        {
            this.configuration = configuration;
            this.environment = env;
        }

        /// <summary>
        /// Gets 加载顺序，越低越先
        /// </summary>
        public virtual int Order => 5;

        /// <summary>
        /// Gets 加载级别
        /// </summary>
        public virtual PackLevel Level => PackLevel.Application;

        /// <summary>
        /// Gets a value indicating whether gets 启用
        /// </summary>
        public virtual bool IsEnabled => true;

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns></returns>
        public virtual IServiceCollection AddServices(IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">服务提供者</param>
        public virtual void UsePack(IApplicationBuilder app)
        {
        }
    }
}