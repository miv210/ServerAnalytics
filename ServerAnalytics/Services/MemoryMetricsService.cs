using ServerAnalytics.Services.Interface;
using ServerAnalytics.Models;
using System.Diagnostics;

namespace ServerAnalytics.Services
{
    public class MemoryMetricsService : IMemoryMetricsService
    {
        public ServerAnalyticsContext db;
        public IRuntimeInformation runtimeInformation { get; set; }
        public MemoryMetricsService(IRuntimeInformation runtimeInformation)
        {
            this.runtimeInformation = runtimeInformation;
        }
        public List<MemoryMetric> GetMemoryMetric()
        {
            if (runtimeInformation.IsUnix())
            {
                return GetUnixMetrics();
            }
            return GetWindowsMetrics();
        }
        private List<MemoryMetric> GetWindowsMetrics()
        {
            List<MemoryMetric> memoryMetrics = new List<MemoryMetric>();

            using (db = new ServerAnalyticsContext())
            {
                memoryMetrics = db.MemoryMetrics.ToList();
            }
            return memoryMetrics;
        }
        private List<MemoryMetric> GetUnixMetrics()
        {
            List<MemoryMetric> memoryMetrics = new List<MemoryMetric>();

            using (db = new ServerAnalyticsContext())
            {
                memoryMetrics = db.MemoryMetrics.ToList();
            }
            return memoryMetrics;
        }
        public void UpdateMemoryMetrics(MemoryMetric memoryMetric)
        {
            if (runtimeInformation.IsUnix())
            {
                UpdateUnixMetrics(memoryMetric);
            }
            UpdateWindowsMetrics(memoryMetric);
        }
        
        private void UpdateWindowsMetrics(MemoryMetric memoryMetric)
        {

            using (db = new ServerAnalyticsContext())
            {
                db.MemoryMetrics.Add(memoryMetric);
                db.SaveChanges();
            }
        }

        
        private void UpdateUnixMetrics(MemoryMetric memoryMetric)
        {
            using (db = new ServerAnalyticsContext())
            {
                db.MemoryMetrics.Add(memoryMetric);
                db.SaveChanges();
            }
        }
    }
}
