// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Auth;
    using Qx.Sprite.Core;

    /// <summary>
    /// 注入userinfo和multitenant的包
    /// </summary>
    /// <typeparam name="TUKey">用户主键</typeparam>
    /// <typeparam name="TTKey">商户主键</typeparam>
    [TodoPack]
    public abstract class TenantUserPack<TUKey, TTKey> : AspNetPack
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantUserPack{TUKey, TTKey}"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public TenantUserPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override int Order => base.Order - 2;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserInfo>(this.UserFactory);
            services.AddScoped<IMultiTenant>(this.UserFactory);
            return base.AddServices(services);
        }

        private JwtUser<TUKey, TTKey> UserFactory(IServiceProvider provider)
        {
            var context = provider.GetService<IHttpContextAccessor>();
            var auth = context?.HttpContext?.Request.Headers.Authorization.ToString();
            var tenantId = context?.HttpContext?.Request.Headers["TenantId"].ToString();
            var defalutUser = new JwtUser<TUKey, TTKey>();
            if (tenantId != null)
            {
                try
                {
                    defalutUser.TenantId = (TTKey)Convert.ChangeType(tenantId, typeof(TTKey));
                }
                catch (Exception)
                {
                }
            }

            if (auth.IsNullOrEmpty() || !auth.StartsWith($"{JwtBearerDefaults.AuthenticationScheme} "))
            {
                return defalutUser;
            }
            else
            {
                var token = auth.Substring($"{JwtBearerDefaults.AuthenticationScheme} ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                try
                {
                    // 验证并读取令牌
                    var claims = handler.ValidateToken(token, JwtAuthenticationPack.TokenValidationParameters, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);
                    if (claims != null)
                    {
                        var user = new JwtUser<TUKey, TTKey>(claims.Claims);
                        return user;
                    }
                    else
                    {
                        return defalutUser;
                    }
                }
                catch
                {
                    return defalutUser;
                }
            }
        }
    }
}