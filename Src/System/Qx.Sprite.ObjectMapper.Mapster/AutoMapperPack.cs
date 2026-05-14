// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.ObjectMapper.Mapster
{
    using System;
    using global::Mapster;
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
            services.AddMapster();

            var find = new AppDomainAssemblyFinder();
            var converters = find.FindTypesInheritFrom<IMapperConfiguration>();
            foreach (var item in converters)
            {
                var add = Activator.CreateInstance(item) as IMapperConfiguration;
                if (!add.IsNullOrEmpty()) add.AddMapper(TypeAdapterConfig.GlobalSettings);
            }

            return services;
        }
    }
}