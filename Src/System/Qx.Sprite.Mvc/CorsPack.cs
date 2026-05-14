// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;

    /// <summary>
    /// 跨域配置包
    /// </summary>
    public class CorsPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorsPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public CorsPack(IConfiguration configuration, IHostEnvironment env)
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
            var hosts = this.configuration.Get<string[]>("AspNetPack:CorsHosts");
            if (hosts?.Any() == true)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(
                        "default",
                        builder =>
                        {
                            builder.WithOrigins(hosts)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                        });
                });
            }

            return services;
        }
    }
}