// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Log
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Qx.Sprite.Core;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// Serilog 日志记录器包
    /// </summary>
    public class SerilogLoggerPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLoggerPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public SerilogLoggerPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override int Order => 0;

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.Core;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            var minimumLevel = this.configuration.Get<Dictionary<string, LogEventLevel>>("Logging:LogLevel");
            services.AddSerilog((services, config) =>
            {
                if (minimumLevel.IsNotNull())
                {
                    foreach (var item in minimumLevel)
                    {
                        config.MinimumLevel.Override(item.Key, item.Value);
                    }
                }

                config
                .ReadFrom.Configuration(this.configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.SQLite(sqliteDbPath: "App_Data/log.db", tableName: "Logs", storeTimestampInUtc: false)
                .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff} [{Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}");
            });

            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            // Write streamlined request completion events, instead of the more verbose ones from the framework.
            // To use the default framework request logging instead, remove this line and set the "Microsoft"
            // level in appsettings.json to "Information".
            app.UseSerilogRequestLogging();

            base.UsePack(app);
        }
    }
}