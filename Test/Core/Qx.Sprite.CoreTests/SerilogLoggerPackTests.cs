using Xunit;
using Qx.Sprite.Log;

namespace Qx.Sprite.Log.Tests
{
    public class SerilogLoggerPackTests
    {
        [Fact()]
        public void SerilogSQLiteTest()
        {
            var builder = WebApplication.CreateSlimBuilder();
            new SerilogLoggerPack(builder.Configuration, builder.Environment).AddServices(builder.Services);

            var app = builder.Build();

            // 在 app 构建后，使用 app.Logger 进行日志记录
            var logger = app.Services.GetService<ILogger<SerilogLoggerPackTests>>();
            logger?.LogInformation("Hello World!");
        }

        [Fact()]
        public void Log4NetTest()
        {
            var builder = WebApplication.CreateSlimBuilder();
            new Log4NetLoggerPack(builder.Configuration, builder.Environment).AddServices(builder.Services);

            var app = builder.Build();

            // 在 app 构建后，使用 app.Logger 进行日志记录
            var logger = app.Services.GetService<ILogger<SerilogLoggerPackTests>>();
            logger?.LogInformation("Hello World!");
        }
    }
}