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
    using Microsoft.Extensions.Options;

    /// <summary>
    /// ServeInfoPack
    /// </summary>
    public class ServeInfoPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServeInfoPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public ServeInfoPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.Configure<ServeInfo>(this.configuration.GetSection("AspNetPack:ServeInfo"));
            services.AddScoped<IServeInfo, ServeInfo>(provider => provider.GetService<IOptions<ServeInfo>>()?.Value ?? new());
            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            base.UsePack(app);
        }
    }
}