using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;

namespace ServerAnalytics.Services
{
    public class UpdateData
    {
        private static UpdateData instance;

        ServerAnalyticsContext db;
        IRunningProcessesService runningService;
        IMemoryMetricsService memoryMetricsService;
        IProcessorMetricsService processorMetricsService;
        private UpdateData(IRunningProcessesService runningService, IMemoryMetricsService memoryMetricsService, IProcessorMetricsService processorMetricsService) 
        {
            this.runningService = runningService;
            this.memoryMetricsService = memoryMetricsService;
            this.processorMetricsService = processorMetricsService;
        }

        public void Update() 
        {
            List<Server> listServers = new List<Server>();
            using (db = new ServerAnalyticsContext()) 
            {
                listServers = db.Servers.ToList();                                                   
            }

            if (listServers.Count == 0) 
            {
                Console.WriteLine("Добавте сервер");
            }
            //foreach (Server server in listServers)
            //{
            //    runningService.UpdateRunningProcesses();
            //    memoryMetricsService.UpdateMemoryMetrics();
            //    processorMetricsService.UpdateCpuMetrics();
            //}
        }

        public static UpdateData getInstance(IRunningProcessesService service, IMemoryMetricsService memoryMetricsService, IProcessorMetricsService processorMetricsService)
        {
            if (instance == null)
            {
                instance = new UpdateData(service, memoryMetricsService, processorMetricsService);
            }
            return instance;
        }
    }
}
