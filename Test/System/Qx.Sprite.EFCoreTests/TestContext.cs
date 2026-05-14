namespace Qx.Sprite.EFCore.Tests
{
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.EFCore;

    public class TestContext : AppDbContext
    {
        public TestContext()
        { }

        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        protected override string Schema => "test";
        protected override string TableNamePrefix => "base";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=test;Username=postgres;Password=aaaa1234");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}