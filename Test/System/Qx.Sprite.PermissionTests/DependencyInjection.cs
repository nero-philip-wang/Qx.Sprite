namespace Qx.Sprite.Core
{
    using Xunit;

    [CollectionDefinition("DependencyInjectionCollection")]
    public class DependencyInjectionCollection : ICollectionFixture<DependencyInjectionFixture>
    { }

    public class DependencyInjectionFixture : IDisposable
    {
        private static int port = 5000;
        private readonly WebApplication app;

        public DependencyInjectionFixture()
        {
            var builder = WebApplication.CreateBuilder([]);
            builder.WebHost.UseUrls($"http://localhost:{port++}");
            builder.Host.AddPack();

            this.app = builder.Build();
            app.UsePack();

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