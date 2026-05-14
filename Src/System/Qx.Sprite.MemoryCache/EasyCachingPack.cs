// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Caching
{
    using System;
    using EasyCaching.Core;
    using EasyCaching.Core.Configurations;
    using EasyCaching.Core.DistributedLock;
    using EasyCaching.Core.Interceptor;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;

    /// <summary>
    /// EasyCaching 缓存包
    /// </summary>
    public class EasyCachingPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EasyCachingPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public EasyCachingPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddEasyCaching(options =>
            {
                if (this.configuration["easycaching:redis"] != null)
                {
                    // 使用Redis缓存
                    options.UseRedis(this.configuration, "default");
                }
                else
                {
                    // 默认使用内存缓存
                    options.UseInMemory("default");
                }
            });

            this.Interceptor(services);

            return base.AddServices(services);
        }

        private void Interceptor(IServiceCollection services)
        {
            services.AddSingleton<IEasyCachingKeyGenerator, DefaultEasyCachingKeyGenerator>();

            var finder = new AppDomainAssemblyFinder();
            var cached = finder.FindTypes(finder.GetAllAssemblies(), c => c.HasAttribute<EasyCachingMarkAttribute>());
            foreach (var type in cached)
            {
                services.Decorate(type, (target, provider) => CacheInterceptorProxy.Create(
                    target,
                    type,
                    provider.GetService<IEasyCachingProvider>(),
                    Options.Create(new EasyCachingInterceptorOptions() { CacheProviderName = "default" }),
                    provider.GetService<IEasyCachingKeyGenerator>()));
            }
        }
    }
}