namespace Qx.Sprite.TestHost
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Hosting;

    public class TestHostApplicationBuilder : IApplicationBuilder
    {
        private readonly IHost host;

        public TestHostApplicationBuilder(IHost host)
        {
            ApplicationServices = host.Services;
            this.host = host;
        }

        public IServiceProvider ApplicationServices { get; set; }
        public IFeatureCollection ServerFeatures { get; set; }

        public IDictionary<string, object?> Properties { get; set; } = new Dictionary<string, object?>();

        IFeatureCollection IApplicationBuilder.ServerFeatures => throw new NotImplementedException();

        public RequestDelegate Build()
        {
            return context =>
            {
                return Task.CompletedTask;
            };
        }

        public IApplicationBuilder New()
        {
            return new TestHostApplicationBuilder(host);
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            return this;
        }
    }
}