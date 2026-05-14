// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// 自动化的依赖注入机制
    /// </summary>
    public class AutoDependencyInjectionPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDependencyInjectionPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public AutoDependencyInjectionPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.Core;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            this.GetThenAdd(services, ServiceLifetime.Singleton);
            this.GetThenAdd(services, ServiceLifetime.Scoped);
            this.GetThenAdd(services, ServiceLifetime.Transient);
            return base.AddServices(services);
        }

        /// <summary>
        /// 获取所有程序集中的制定类型,并添加到服务容器中
        /// </summary>
        /// <param name="services"></param>
        /// <param name="lifetime"></param>
        private void GetThenAdd(IServiceCollection services, ServiceLifetime lifetime)
        {
            var type =
                lifetime == ServiceLifetime.Transient ? typeof(ITransient) :
                lifetime == ServiceLifetime.Scoped ? typeof(IScoped) :
                typeof(ISingleton);

            var finder = new AppDomainAssemblyFinder();
            var types = finder.FindTypesInheritFrom(type, false);
            foreach (var item in types)
            {
                if (item.HasAttribute<ServiceTypeAttribute>())
                {
                    var attrs = item.GetAttribute<ServiceTypeAttribute>(false);

                    // 支持多个 SpecificInterfaceAttribute
                    foreach (var attr in attrs)
                    {
                        services.Add(new ServiceDescriptor(attr.Type ?? item, item, lifetime));
                        if (attr.Type != null && !attr.Key.IsNullOrEmpty())
                        {
                            services.Add(new ServiceDescriptor(attr.Type, attr.Key, item, lifetime));
                        }
                    }
                }
                else
                {
                    var serviceType = item.GetInterfaces().Where(it => it.Name != type.Name).FirstOrDefault();
                    services.Add(new ServiceDescriptor(serviceType ?? item, item, lifetime));
                }
            }
        }
    }
}