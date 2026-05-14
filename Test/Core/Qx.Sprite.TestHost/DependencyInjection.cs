namespace Qx.Sprite.Core
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Hosting;
    using Qx.Sprite.TestHost;
    using Xunit;

    [CollectionDefinition("DependencyInjectionCollection")]
    public class DependencyInjectionCollection : ICollectionFixture<DependencyInjectionFixture>
    { }

    public class DependencyInjectionFixture : IDisposable
    {
        private readonly IHost app;

        public DependencyInjectionFixture()
        {
            var builder = Host.CreateDefaultBuilder([]);
            builder.AddPack();

            app = builder.Build();
            var appBuilder = new TestHostApplicationBuilder(app);
            appBuilder.UsePack();
            Service = app.Services;
            app.Start();
        }

        public IServiceProvider Service { get; }

        public void Dispose()
        {
            app.StopAsync().Wait();
        }
    }
}