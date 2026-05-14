// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 包管理器
    /// </summary>
    public class PackManager
    {
        /// <summary>
        /// Gets 原始包
        /// </summary>
        public List<AspNetPack> SourcePacks { get; } = new List<AspNetPack>();

        /// <summary>
        /// Gets 已加载包
        /// </summary>
        public List<AspNetPack> LoadedPacks { get; } = new List<AspNetPack>();

        /// <summary>
        /// Gets or sets 抽象包名
        /// </summary>
        public List<string> AbstractPacks { get; set; } = new List<string>();

        /// <summary>
        /// 加载模块服务
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public IServiceCollection AddPacks(IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            // 初始化 AppDomainAssemblyFinder
            var prefix = configuration.Get<IEnumerable<string>>("AspNetPack:Core:FilePrefix");
            if (prefix.IsNullOrEmpty())
                throw new Exception("配置文件中AspNetPack:Core:FilePrefix节点不能为空");
            AppDomainAssemblyFinder.InitFilePrefix(prefix);
            var finder = new AppDomainAssemblyFinder();
            var assemblies = finder.GetAllAssemblies();

            // 获取所有的包
            var packs = finder.FindTypesInheritFrom<AspNetPack>(true);
            var realPacks = packs.Where(c => !c.IsAbstract).ToArray();

            this.SourcePacks.Clear();
            this.SourcePacks.AddRange(realPacks.Select(m => (AspNetPack)Activator.CreateInstance(m, configuration, env)!));

            this.AbstractPacks = packs.Where(c =>
                c.IsAbstract &&
                c.HasAttribute<TodoPackAttribute>() &&
                !this.SourcePacks.Any(d => c.IsAssignableFrom(d.GetType()))).Select(c => c.Name).ToList();

            this.LoadedPacks.Clear();
            this.LoadedPacks.AddRange(this.SourcePacks.Where(i => i.IsEnabled).OrderBy(i => i.Level).ThenBy(i => i.Order));

            foreach (var item in this.LoadedPacks)
            {
                services = item.AddServices(services);
            }

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">服务提供者</param>
        public virtual void UsePack(IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetLogger<PackManager>();
            if (this.AbstractPacks.Any())
            {
                logger.LogWarning("以下模块需要您继承并实现抽象方法：{0}", this.AbstractPacks.Join(Environment.NewLine));
            }

            logger.LogInformation("----------------------Qx.Sprite.Core框架初始化开始");
            var watch = Stopwatch.StartNew();
            var cellWatch = Stopwatch.StartNew();

            foreach (var pack in this.LoadedPacks)
            {
                logger.LogInformation($"----------------------模块[{(int)pack.Level}-{pack.Order}]加载中: {pack.GetType().Name}");
                cellWatch.Restart();
                try
                {
                    pack.UsePack(app);
                }
                catch (Exception)
                {
                    logger.LogError($"----------------------模块加载失败: {pack.GetType().Name}");
                    throw;
                }

                logger.LogInformation($"----------------------模块加载成功: {pack.GetType().Name}，耗时：{cellWatch.ElapsedMilliseconds}");
            }

            watch.Stop();
            logger.LogInformation($"----------------------Qx.Sprite.Core框架初始化完成，耗时：{watch.ElapsedMilliseconds}毫秒");
        }
    }
}