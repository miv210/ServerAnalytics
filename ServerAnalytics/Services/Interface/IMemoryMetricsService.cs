using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IMemoryMetricsService
    {
        public List<MemoryMetric> GetMemoryMetric();
        void UpdateMemoryMetrics(MemoryMetric memoryMetric);
    }
}
