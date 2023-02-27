using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IProcessorMetricsService
    {
        Task<WorkLodaProcessor> GetCpuUsageForProcessAsync();
    }
}
