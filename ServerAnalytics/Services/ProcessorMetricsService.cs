using System.Diagnostics;
using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;

namespace ServerAnalytics.Services
{
    public class  ProcessorMetricsService : IProcessorMetricsService
    {
        public async Task<WorkLodaProcessor> GetCpuUsageForProcessAsync()
        {
            WorkLodaProcessor processor = new WorkLodaProcessor();

            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            await Task.Delay(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            processor.WorkLoda = cpuUsageTotal * 100;
            processor.DateCheck = DateTime.UtcNow;

            return  processor;
        }
    }
}
