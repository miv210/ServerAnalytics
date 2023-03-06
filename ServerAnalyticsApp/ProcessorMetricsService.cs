using System.Diagnostics;


namespace ServerAnalytics.Services
{
    public class  ProcessorMetricsService
    {
        //ServerAnalyticsContext db;

        //public List<WorkLodaProcessor> GetCpuMetrics()
        //{
        //    List<WorkLodaProcessor> workLodaProcessors= new List<WorkLodaProcessor>();

        //    using (db = new ServerAnalyticsContext())
        //    {
        //        workLodaProcessors = db.WorkLodaProcessors.ToList();
        //    }
        //    return workLodaProcessors;
        //}

        public async Task UpdateCpuMetrics()
        {
            //WorkLodaProcessor processor;

            Process[] listProcessec = Process.GetProcesses("http://localhost:5232/172.30.16.1");

            double cpuUsageTotal = 1;
            foreach ( Process process in listProcessec)
            {
                var startTime = DateTime.UtcNow;
                var startCpuUsage = process.TotalProcessorTime;
                await Task.Delay(500);
                var endTime = DateTime.UtcNow;
                var endCpuUsage = process.TotalProcessorTime;
                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                cpuUsageTotal =+ cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
                Console.WriteLine($"{cpuUsageTotal}");
            }

            //processor = new WorkLodaProcessor
            //{
            //    WorkLoda =cpuUsageTotal,
            //    DateCheck = DateTime.Now,
            //};

            //Console.WriteLine($"{processor.WorkLoda} {processor.DateCheck}");
            //using (db = new ServerAnalyticsContext())
            //{
            //    db.WorkLodaProcessors.Add(processor);
            //    db.SaveChanges();
            //}
        }
    }
}
