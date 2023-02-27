using Microsoft.EntityFrameworkCore;

namespace ServerAnalytics.Models
{
    public class ServerAnalyticsContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public ServerAnalyticsContext()
        {
            Database.EnsureDeleted();
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

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<MemoryMetric> MemoryMetrics { get; set; } = null!;

        public DbSet<WorkLodaProcessor> WorkLodaProcessors { get; set;} = null!;
        public DbSet<RunningProcess> RunningProcesses { get; set; } = null!;
    }
}
