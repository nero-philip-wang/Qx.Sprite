namespace Demo
{
    using Consul;
    using DotNetCore.CAP;
    using Hangfire;
    using Hangfire.PostgreSql;
    using Qx.Sprite.BackgroundJob;
    using Qx.Sprite.Cap;

    public class AspNetPackConfig
    {

        public static void InitAll()
        {
            HangfirePack.ConfigStorage = (instance, configuration, connectionString) =>
            {
                return configuration.UsePostgreSqlStorage(options =>
                {
                    options.UseNpgsqlConnection(connectionString);
                });
            };

            CapPack.ConfigCap = (CapPack instance, CapOptions options, IConfiguration configuration) =>
            {
                return options
                .UsePostgreSql(configuration.GetConnectionString("DefaultConnection"))
                .UseRabbitMQ(options =>
                {
                    options.HostName = "localhost";
                    options.UserName = "user";
                    options.Password = "password";
                    options.Port = 5673;
                });
            };
        }
    }
}
