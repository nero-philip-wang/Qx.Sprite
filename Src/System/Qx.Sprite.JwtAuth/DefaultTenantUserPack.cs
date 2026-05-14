// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.ApiFx.Auth
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// 默认商户用户包，使用long类型的用户主键和string类型的商户主键
    /// </summary>
    public class DefaultTenantUserPack : Qx.Sprite.Auth.TenantUserPack<long, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTenantUserPack"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public DefaultTenantUserPack(IConfiguration configuration, IHostEnvironment env)
            : base(configuration, env)
        {
        }
    }
}