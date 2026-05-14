// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.BackgroundJob
{
    using System.Diagnostics.CodeAnalysis;
    using Hangfire.Dashboard;

    /// <summary>
    /// 运行界面匿名授权过滤器
    /// </summary>
    public class AnonymousAuthorizeFilter : IDashboardAuthorizationFilter
    {
        /// <inheritdoc/>
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}