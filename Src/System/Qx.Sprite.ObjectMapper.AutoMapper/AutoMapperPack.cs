// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.ObjectMapper
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;

    /// <summary>
    /// AutoMapper模块
    /// </summary>
    public class AutoMapperPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public AutoMapperPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            var find = new AppDomainAssemblyFinder();
            var converters = find.FindTypesInheritFrom<IMapperConfiguration>();

            services.AddAutoMapper(
                cfg =>
                {
                    foreach (var item in converters)
                    {
                        var add = Activator.CreateInstance(item) as IMapperConfiguration;
                        if (!add.IsNullOrEmpty()) add.AddAutoMapper(cfg);
                    }
                },
                find.GetAllAssemblies());
            return services;
        }
    }
}