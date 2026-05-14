// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;
    using Senparc.CO2NET;
    using Senparc.CO2NET.RegisterServices;
    using Senparc.Weixin;
    using Senparc.Weixin.Entities;
    using Senparc.Weixin.RegisterServices;

    public class WeChatOAuthPack : AspNetPack
    {
        public WeChatOAuthPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        public override PackLevel Level => PackLevel.Application;

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            // 如果配置文件有 UseSqlServe就读取数据库
            var keys = this.configuration.Get<OAuthKey[]>("AspNetPack:WeChat:Options");
            if (!keys.IsNullOrEmpty()) services.AddScoped<IEnumerable<OAuthKey>>(p => keys);

            // 如果有参数就读取配置文件的
            var useDb = this.configuration["AspNetPack:WeChat:UseSqlServe"];
            if (!useDb.IsNullOrEmpty()) services.AddScoped<IEnumerable<OAuthKey>>(p => p.GetService<IEfRepository<int, OAuthKey>>()!);

            services.AddMemoryCache();
            services
                .AddSenparcGlobalServices(this.configuration)
                .AddSenparcWeixinServices(this.configuration);

            return base.AddServices(services);
        }

        public override void UsePack(IApplicationBuilder app)
        {
            var senparcSetting = app.ApplicationServices.GetService<IOptions<SenparcSetting>>();
            var senparcWeixinSetting = app.ApplicationServices.GetService<IOptions<SenparcWeixinSetting>>();
            IRegisterService register = RegisterService.Start(senparcSetting?.Value).UseSenparcGlobal();
            register.UseSenparcWeixin(senparcWeixinSetting?.Value);
            base.UsePack(app);
        }
    }
}