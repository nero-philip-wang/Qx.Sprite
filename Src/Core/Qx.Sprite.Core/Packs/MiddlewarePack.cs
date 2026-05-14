// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// 中间件注入包
    /// </summary>
    /// <typeparam name="T"> 中间件类型 </typeparam>
    public abstract class MiddlewarePack<T> : AspNetPack
         where T : class, IMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiddlewarePack{T}"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public MiddlewarePack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override int Order => base.Order - 1;

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<T>();
            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseMiddleware<T>();
            base.UsePack(app);
        }
    }
}