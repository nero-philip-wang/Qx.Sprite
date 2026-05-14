// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Log
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Qx.Sprite.Core;

    /// <summary>
    /// 日志模块
    /// </summary>
    public class Log4NetLoggerPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLoggerPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Log4NetLoggerPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.Core;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddSingleton<ILoggerProvider, Log4NetLoggerProvider>();
            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            base.UsePack(app);
        }
    }
}