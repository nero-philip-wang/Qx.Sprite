namespace Qx.Sprite.MemoryCache.Tests
{
    using EasyCaching.Core.Interceptor;
    using Microsoft.Extensions.DependencyInjection;
    using Qx.Sprite.Caching;
    using Qx.Sprite.Core;
    using System.Net;
    using Xunit;

    [EasyCachingMark]
    public interface IService
    {
        [EasyCachingAble]
        string GetData();

        [EasyCachingPut]
        string PutData();
    }

    public class EasyCachingTests : DependencyTestBase
    {
        public EasyCachingTests(DependencyInjectionFixture fixture) : base(fixture)
        {
        }

        [Fact()]
        public void Test()
        {
            var service = provider.GetRequiredService<IService>();
            var data1 = service.GetData();
            Thread.Sleep(1015);
            var data2 = service.GetData();
            Assert.Equal(data1, data2);

            var data4 = service.PutData();
            Assert.NotEqual(data1, data4);

            //Thread.Sleep(30000);
            //var data3 = service.GetData();
            //Assert.NotEqual(data4, data3);
        }
    }

    public class Service : IService, IScoped
    {
        public string GetData() => DateTime.Now.ToString();

        public string PutData() => DateTime.Now.ToFileTime().ToString();
    }
}