namespace Qx.Sprite.Permission
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Options;

    public class PermissionDbContextFactory : IDesignTimeDbContextFactory<PermissionDbContext>
    {
        public static string Conn = "Host=localhost;Database=frame;Username=postgres;Password=aaaa1234;";

        /// <inheritdoc/>
        public PermissionDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PermissionDbContext>();
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseNpgsql(Conn, b => b.MigrationsAssembly("Qx.Sprite.PermissionTests"))
                .UseSnakeCaseNamingConvention();

            return new PermissionDbContext(optionsBuilder.Options);
        }
    }
}
