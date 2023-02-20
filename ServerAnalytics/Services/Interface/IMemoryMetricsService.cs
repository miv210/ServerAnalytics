using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IMemoryMetricsService
    {
        public MemoryMetric GetMemoryMetric();
    }
}
