// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;

    /// <summary>
    /// JwtRoleHandler
    /// </summary>
    public class JwtRoleHandler : AuthorizationHandler<JwtRequirement>, IScoped
    {
        private readonly IUserInfo userInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtRoleHandler"/> class.
        /// </summary>
        /// <param name="userInfo"></param>
        public JwtRoleHandler(IUserInfo userInfo)
        {
            this.userInfo = userInfo;
        }

        /// <inheritdoc/>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            var aud = context.User.Claims.FirstOrDefault(c => c.Type == "aud")?.Value;
            var allowArea = aud?.Split(',');
            try
            {
                if (context.Resource is HttpContext httpContext && allowArea.IsNotNull())
                {
                    var area = httpContext.Request.RouteValues["area"]?.ToString();

                    if (area == null || allowArea.Contains(area))
                    {
                        context.Succeed(requirement);
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }
    }
}