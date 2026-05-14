namespace Qx.Sprite.EFCore.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.EFCore;

    public class TestContextPack : AppDbContextPack<TestContext>
    {
        public TestContextPack(IConfiguration configuration, IHostEnvironment env) : base(configuration, env)
        {
        }

        protected override DbContextOptionsBuilder UseServe(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=test.db;");
            return builder;
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddScoped<IMultiTenant>(p => null!);
            services.AddScoped<IUserInfo>(p => null!);
            return base.AddServices(services);
        }

        public override void UsePack(IApplicationBuilder app)
        {
            var scoped = app.ApplicationServices.CreateScope().ServiceProvider;
            var db = scoped.GetService<TestContext>();
            var result = db.Database.EnsureCreated();
            base.UsePack(app);
        }
    }
}