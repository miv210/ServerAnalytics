using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IProcessorMetricsService
    {
        List<WorkLodaProcessor> GetCpuMetrics();
        Task UpdateCpuMetrics();
    }
}
