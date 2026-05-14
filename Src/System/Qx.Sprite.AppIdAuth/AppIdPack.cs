// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;

    /// <summary>
    /// APPID 认证包
    /// </summary>
    public class AppIdPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public AppIdPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override int Order => base.Order - 1;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            // 添加授权
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "AppId";
                options.DefaultChallengeScheme = "AppId";
            }).AddJwtBearer("AppId", "AppId", options =>
            {
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("appid", policy =>
                {
                    policy.AddAuthenticationSchemes("AppId");
                    policy.AddRequirements(new AppIdRequirement());
                });
            });

            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            base.UsePack(app);
        }
    }
}