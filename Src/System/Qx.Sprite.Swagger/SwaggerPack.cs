// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Swagger
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Qx.Sprite.Core;

    /// <summary>
    /// Swagger模块类
    /// </summary>
    public class SwaggerPack : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public SwaggerPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override int Order => base.Order - 1;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            bool enabled = true;
            if (!enabled)
            {
                return services;
            }

            string? url = this.configuration["AspNetPack:Swagger:Url"];
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("配置文件中Swagger节点的Url不能为空");
            }

            string? title = this.configuration["AspNetPack:Swagger:Title"];
            int version = 1;

            services.AddSwaggerGen(options =>
            {
                // 加载xml
                options.SwaggerDoc($"v{version}", new OpenApiInfo() { Title = title, Version = $"v{version}" });
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file);
                });

                // 配置文档默认值
                options.SchemaFilter<DefaultValueSchemaFilter>();

                // 按控制器分组
                options.TagActionsBy(c =>
                    {
                        var pos = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            var newp = c.RelativePath!.IndexOf('/', pos + 1);
                            if (newp == -1)
                            {
                                pos = c.RelativePath.Length;
                                break;
                            }
                            else
                            {
                                pos = newp;
                            }
                        }

                        return [c.RelativePath!.Substring(0, pos)];
                    });

                // 权限Token
                var authorization = new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "请输入带有Bearer的Token，形如 “Bearer {Token}” ",
                    Name = "Authorization",
                    Reference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    },
                };
                var appid = new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "AppId",
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference()
                    {
                        Id = "AppId",
                        Type = ReferenceType.SecurityScheme,
                    },
                };
                var tenant = new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "TenantId",
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference()
                    {
                        Id = "TenantId",
                        Type = ReferenceType.SecurityScheme,
                    },
                };
                options.AddSecurityDefinition("Bearer", authorization);
                options.AddSecurityDefinition("AppId", appid);
                options.AddSecurityDefinition("TenantId", tenant);
                var req = new OpenApiSecurityRequirement() { [authorization] = Enumerable.Empty<string>().ToList() };
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    { authorization, Enumerable.Empty<string>().ToList() },
                    { appid, new[] { "default" } },
                    { tenant, new[] { "default" } },
                });
            });
            services.AddSwaggerGenNewtonsoftSupport();
            return services;
        }

        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">Asp应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            bool enabled = true;
            if (!enabled)
            {
                return;
            }

            app.UseSwagger().UseSwaggerUI(options =>
            {
                string? url = this.configuration["AspNetPack:Swagger:Url"];
                string? title = this.configuration["AspNetPack:Swagger:Title"];
                int version = 1;
                options.SwaggerEndpoint(url, $"{title} V{version}");
            });
        }
    }
}