// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// 默认Web应用程序
    /// </summary>
    public static class DefaultWebApplication
    {
        private const string ConfigPath = "App_Data/config/";

        /// <summary>
        /// Gets or sets 包管理器
        /// </summary>
        public static PackManager PackManager { get; set; } = new PackManager();

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IHostBuilder AddPack(this IHostBuilder host)
        {
            return host
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices((hostingContext, services) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    var conf = hostingContext.Configuration;
                    PackManager.AddPacks(services, conf, env);
                });
        }

        /// <summary>
        /// 使用服务
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePack(this IApplicationBuilder builder)
        {
            PackManager.UsePack(builder);
            return builder;
        }

        private static void ConfigureAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            var env = hostingContext.HostingEnvironment;
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigPath);
            Directory.CreateDirectory(dir);
            var filepaths = Directory.EnumerateFiles(dir);
            filepaths = filepaths
                .Select(c => Path.GetFileName(c))
                .Where(c => c.Count(d => d == '.') == 1 || c.Contains(env.EnvironmentName))
                .OrderBy(c => c.Length);

            foreach (var fp in filepaths)
            {
                config.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigPath + fp));
            }

            config.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json"), false, true);
            config.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"appsettings.{env.EnvironmentName}.json"), true, true);
        }
    }
}