// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Cap
{
    using System;
    using System.Threading.Tasks;
    using DotNetCore.CAP.Filter;
    using Microsoft.Extensions.DependencyInjection;
    using Qx.Sprite.Core;

    /// <summary>
    /// 通过 CAP 消息头设置租户和用户信息的订阅过滤器
    /// </summary>
    public class CapHeaderFilter : SubscribeFilter
    {
        private readonly IServiceProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapHeaderFilter"/> class.
        /// </summary>
        /// <param name="provider"></param>
        public CapHeaderFilter(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// 订阅方法执行前
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task OnSubscribeExecutingAsync(ExecutingContext context)
        {
            var tenant = this.provider.GetService<ITenantInfo>();
            var userInfo = this.provider.GetService<IUserInfo>();

            context.DeliverMessage.Headers.TryGetValue("TenantId", out var tenantId);
            context.DeliverMessage.Headers.TryGetValue("UserId", out var userId);
            if (!string.IsNullOrEmpty(tenantId))
            {
                tenant?.TenantId = tenantId;
            }

            if (!string.IsNullOrEmpty(userId))
            {
                userInfo?.Id = userId;
            }

            return Task.CompletedTask;
        }
    }
}