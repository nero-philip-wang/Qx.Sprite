namespace Qx.Sprite.Core
{
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    [Collection("DependencyInjectionCollection")]
    public class DependencyTestBase : IClassFixture<DependencyInjectionFixture>
    {
        protected readonly IServiceProvider provider;

        public DependencyTestBase(DependencyInjectionFixture fixture)
        {
            provider = fixture.Service.CreateScope().ServiceProvider;
        }
    }
}