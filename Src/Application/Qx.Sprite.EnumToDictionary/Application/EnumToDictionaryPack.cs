// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionary
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;

    /// <summary>
    /// 枚举到字典包
    /// </summary>
    public abstract class EnumToDictionaryPack(IConfiguration configuration, IHostEnvironment env) : AspNetPack(configuration, env)
    {
        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.Application;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IEnumerable<IDictionary>>(this.GetItems);
            return base.AddServices(services);
        }

        /// <summary>
        /// 读取所有枚举到字典的映射关系，并更新到数据库
        /// </summary>
        /// <param name="app"></param>
        public override void UsePack(IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.CreateScope().ServiceProvider;
            var service = provider.GetService<IEnumToDictionaryService>();
            service.CheckNotNull(nameof(EnumToDictionaryService));

            var src = service.FindAllEnum();
            var dest = this.GetItems(app.ApplicationServices.CreateScope().ServiceProvider);

            service.UpdateList(src, dest);
            this.SetItems(dest);

            base.UsePack(app);
        }

        /// <summary>
        /// 保存数据库
        /// </summary>
        protected abstract void SetItems(IEnumerable<IDictionary> dictionary);

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        protected abstract IList<IDictionary> GetItems(IServiceProvider provider);
    }
}