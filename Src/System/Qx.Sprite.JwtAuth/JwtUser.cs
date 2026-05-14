// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using Microsoft.AspNetCore.Http;
    using Microsoft.IdentityModel.Tokens;
    using Qx.Sprite.Core;

    /// <summary>
    /// 带有商户信息的jwt用户
    /// </summary>
    /// <typeparam name="TUKey">用户主键</typeparam>
    /// <typeparam name="TTKey">商户主键</typeparam>
    public class JwtUser<TUKey, TTKey> : IUserInfo<TUKey>, IMultiTenant<TTKey>, IHasExtraData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JwtUser{TUKey, TTKey}"/> class.
        /// </summary>
        public JwtUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtUser{TUKey, TTKey}"/> class.
        /// </summary>
        /// <param name="claims"></param>
        public JwtUser(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                var name = claim.Type;
                var value = claim.Value;
                var property = this.GetType().GetProperty(name);
                if (property.IsNotNull())
                {
                    if (property.PropertyType == typeof(string))
                        property.SetValue(this, value);
                    else if (property.PropertyType.IsValueType)
                        property.SetValue(this, Convert.ChangeType(value, property.PropertyType));
                    else
                        property.SetValue(this, value.FromJsonString(property.PropertyType));
                }
                else
                {
                    this.ExtraData[name] = value;
                }
            }
        }

        /// <inheritdoc/>
        public TUKey? Id { get; set; }

        /// <inheritdoc/>
        public TUKey? UnionId { get; set; }

        /// <inheritdoc/>
        object? IUserInfo.Id { get => this.Id; set => this.Id = value == null ? default : value.As<TUKey>(); }

        /// <inheritdoc/>
        object? IUserInfo.UnionId => this.UnionId;

        /// <inheritdoc/>
        public string? Title { get; set; }

        /// <inheritdoc/>
        public List<object> Role { get; set; } = [];

        /// <inheritdoc/>
        public TTKey? TenantId { get; set; }

        /// <inheritdoc/>
        object? IMultiTenant.TenantId { get => this.TenantId; set => this.TenantId = value == null ? default : value.As<TTKey>(); }

        /// <inheritdoc/>
        public Dictionary<string, object?> ExtraData { get; set; } = [];

        /// <summary>
        /// 转换为Claims
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Claim> ToClaims()
        {
            var claims = this.GetType().GetProperties().Where(c => c.CanRead && c.GetGetMethod(nonPublic: false) != null)
                .Select(c => new Claim(
                    c.Name,
                    (c.PropertyType.IsValueType || c.PropertyType == typeof(string) ? c.GetValue(this)?.ToString() : c.GetValue(this)?.ToJsonString()) ?? string.Empty));
            return claims;
        }

        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="expire"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public string CreateToken(DateTime expire, string audience)
        {
            var handler = new JwtSecurityTokenHandler();
            var claims = this.ToClaims();
            ClaimsIdentity identity = new ClaimsIdentity(claims);
            var token = handler.CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Issuer = JwtAuthenticationPack.ISSUER,
                Audience = audience,
                SigningCredentials = new SigningCredentials(JwtAuthenticationPack.TokenValidationParameters?.IssuerSigningKey, SecurityAlgorithms.RsaSha256Signature),
                Subject = identity,
                Expires = expire,
            });
            return token;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[\"{this.Id}\",\"{this.Title}\"]";
        }
    }
}