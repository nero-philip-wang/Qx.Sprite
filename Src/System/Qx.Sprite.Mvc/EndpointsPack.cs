// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.Json.Serialization.Metadata;
    using Asp.Versioning;
    using Mapster;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;

    /// <summary>
    /// MVC配置包
    /// </summary>
    public class EndpointsPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointsPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public EndpointsPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            var jsonSettings = JsonUtils.JsonOptions;
            var config = new TypeAdapterConfig();
            config.NewConfig<JsonSerializerOptions, JsonSerializerOptions>().MaxDepth(5);
            var mapper = new MapsterMapper.Mapper(config);

            services.AddMemoryCache();
            services.AddHealthChecks();
            services.AddSignalR();
            services.AddPortableObjectLocalization(opt => opt.ResourcesPath = "Languages");

            var languages = this.configuration["AspNetPack:Core:Languages"]?.ToString();
            var supportedCultures = languages?.Split(",") ?? new[] { "zh", "en" };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures)
                    .SetDefaultCulture(supportedCultures[0]);
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            services
                .AddRouting(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                })
                .AddControllers(options =>
                {
                    foreach (var item in this.Filters())
                        options.Filters.Add(item);
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Clear();
                    foreach (var converter in jsonSettings.Converters)
                        opt.JsonSerializerOptions.Converters.Add(converter);
                    opt.JsonSerializerOptions.Encoder = jsonSettings.Encoder;
                    opt.JsonSerializerOptions.PropertyNamingPolicy = jsonSettings.PropertyNamingPolicy;
                    opt.JsonSerializerOptions.ReferenceHandler = jsonSettings.ReferenceHandler;
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = jsonSettings.DefaultIgnoreCondition;
                    opt.JsonSerializerOptions.NumberHandling = jsonSettings.NumberHandling;
                    opt.JsonSerializerOptions.IncludeFields = jsonSettings.IncludeFields;
                    opt.JsonSerializerOptions.TypeInfoResolver = jsonSettings.TypeInfoResolver;
                })
                .AddControllersAsServices()
                .AddDataAnnotationsLocalization();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            services.AddHttpContextAccessor();

            return services;
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHealthChecks("/ready");

            app.UseRouting();
            app.UseCors("default");
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/ready");
                endpoints.MapControllers();
                endpoints.MapHubs();
                this.ConfigureEndpoint(endpoints);
            });
        }

        /// <summary>
        /// 配置Endpoint
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void ConfigureEndpoint(IEndpointRouteBuilder builder)
        {
        }

        /// <summary>
        /// 配置过滤器
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Type> Filters()
        {
            var finder = new AppDomainAssemblyFinder();
            var filters = finder.FindTypesInheritFrom<IFilterMetadata>();
            return filters;
        }
    }
}