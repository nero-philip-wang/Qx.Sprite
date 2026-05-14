namespace Qx.Sprite.Permission
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.Core;
    using Qx.Sprite.Permission;
    using System;
    using System.Collections.Generic;

    public class AuthPermissionDbContextPack : PermissionDbContextPack
    {
        public AuthPermissionDbContextPack(IConfiguration configuration, IHostEnvironment env) : base(configuration, env)
        {
            base.isToMigrateDB = false;
        }
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            //services.AddScoped<IMultiTenant, DemoMultiTenant>();
            //services.AddScoped<IClientInfo, DemoMultiTenant>();
            //services.AddScoped<IUserInfo, DemoMultiTenant>();
            return base.AddServices(services);
        }

        protected override DbContextOptionsBuilder UseDb(DbContextOptionsBuilder builder, string connectionString)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
            AppContext.SetSwitch("Npgsql.EnableDetailedErrors", true);
            AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            return builder
                .UseNpgsql(connectionString, b => b.MigrationsAssembly("Qx.Sprite.PermissionTests"))
                .UseSnakeCaseNamingConvention();
        }

        public class DemoMultiTenant : IMultiTenant<string>, IUserInfo<long>, IClientInfo
        {
            public string? TenantId { get; set; }

            public string Title { get; set; }

            public ClientType Type { get; set; }

            public Version Version { get; set; }

            public bool IsValidVersion { get; set; }

            public string AppId { get; set; }

            public long Id { get; set; }

            public long UnionId { get; set; }

            public List<object> Role { get; set; }

            object? IMultiTenant.TenantId { get => this.TenantId; set => this.TenantId = value?.ToString(); }

            object? IUserInfo.Id { get => this.Id; set => this.Id = value == null ? default : (long)value; }

            object? IUserInfo.UnionId => UnionId;

            public void Clean()
            {
            }

            public override string ToString()
            {
                return "[0,1]";
            }
        }
    }
}
