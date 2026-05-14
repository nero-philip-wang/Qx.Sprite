namespace Demo.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class TenantUserPack : Qx.Sprite.Auth.TenantUserPack<long, string>
    {
        public TenantUserPack(IConfiguration configuration, IHostEnvironment env) : base(configuration, env)
        {
        }
    }
}