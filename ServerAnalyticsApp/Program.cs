using ServerAnalyticsApp.Models;
using ServerAnalyticsApp.Services;

namespace ServerAnalyticsApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            RunningProcessService runningProcessService = new RunningProcessService();
            ProcessorMetricsService processorMetricsService = new ProcessorMetricsService();
            MemoryMetricsService memoryMetricsService = new MemoryMetricsService();

            while (true){
                //await memoryMetricsService.Update();
                //await processorMetricsService.UpdateCpuMetrics();
                await runningProcessService.UpdateRunningProcesses();
                await Task.Delay(100000);
            }     
        }
    }
}