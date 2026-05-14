// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Qx.Sprite.Core;

    /// <summary>
    /// Jwt 认证包
    /// </summary>
    public class JwtAuthenticationPack : AspNetPack
    {
        /// <summary>
        /// ISSUER
        /// </summary>
        public const string ISSUER = "-";

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtAuthenticationPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public JwtAuthenticationPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }

        /// <summary>
        /// Gets tokenValidationParameters
        /// </summary>
        public static TokenValidationParameters? TokenValidationParameters { get; private set; }

        /// <inheritdoc/>
        public override PackLevel Level => PackLevel.System;

        /// <inheritdoc/>
        public override int Order => base.Order - 1;

        /// <inheritdoc/>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IRSAKey key = new RSAKey();
            TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(key.PrivateKeys), // The signing key must match!
                ValidateIssuer = true,                  // Validate the JWT Issuer (iss) claim
                ValidIssuer = ISSUER,
                ValidateAudience = false,               // Validate the JWT Audience (aud) claim
                ValidateLifetime = true,                // Validate the token expiry
                ClockSkew = TimeSpan.Zero,
            };

            // 添加授权
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = TokenValidationParameters;
                o.Events = new JwtBearerEvents
                {
                    // 处理signalR
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.Value.IsNotNull() && path.Value.ToLower().EndsWith("hub"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("jwt", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new JwtRequirement());
                });
            });

            return base.AddServices(services);
        }

        /// <inheritdoc/>
        public override void UsePack(IApplicationBuilder app)
        {
            base.UsePack(app);
        }
    }
}