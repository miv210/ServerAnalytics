using Microsoft.EntityFrameworkCore;

namespace ServerAnalytics.Models
{
    public class ServerAnalyticsContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public ServerAnalyticsContext()
        {
            Database.EnsureCreated();
            
        }
        public ServerAnalyticsContext(DbContextOptions<ServerAnalyticsContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ServerInfo;Username=postgres;Password=123");
        }

    }
}
