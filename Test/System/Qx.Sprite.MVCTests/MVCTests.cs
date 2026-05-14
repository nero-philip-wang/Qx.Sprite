namespace Qx.Sprite.EFCore.Tests
{
    using Qx.Sprite.Core;
    using System.Net;
    using Xunit;

    public class MVCTests : DependencyTestBase
    {
        public MVCTests(DependencyInjectionFixture fixture) : base(fixture)
        {
        }

        [Fact()]
        public void CurdTest()
        {
            var request = new HttpClient();
            var req = request.GetAsync("http://localhost:5001/").Result;
            var resp = req.Content.ReadAsStringAsync().Result;
        }
    }
}