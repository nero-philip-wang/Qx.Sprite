namespace Qx.Sprite.Core
{
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