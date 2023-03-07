using System.Diagnostics;
using ServerAnalyticsApp.Models;


namespace ServerAnalyticsApp.Services
{
    public class ProcessorMetricsService
    {


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
            WorkLodaProcessor processor;

            Process[] listProcessec = Process.GetProcesses();

            double cpuUsageTotal;


            try
            {
                var startTime = DateTime.UtcNow;
                var startCpuUsage = listProcessec[4].TotalProcessorTime;
                await Task.Delay(500);
                var endTime = DateTime.UtcNow;
                var endCpuUsage = listProcessec[4].TotalProcessorTime;
                var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
                var totalMsPassed = (endTime - startTime).TotalMilliseconds;
                cpuUsageTotal = +cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
                Console.WriteLine($"{cpuUsageTotal}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //foreach ( Process process in listProcessec)
            //{
            //    try
            //    {
            //        var startTime = DateTime.UtcNow;
            //        var startCpuUsage = process.TotalProcessorTime;
            //        await Task.Delay(500);
            //        var endTime = DateTime.UtcNow;
            //        var endCpuUsage = process.TotalProcessorTime;
            //        var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            //        var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            //        cpuUsageTotal = +cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            //        Console.WriteLine($"{cpuUsageTotal}");
            //    }
            //    catch ( Exception ex )
            //    {
            //        Console.WriteLine( ex.Message );
            //    }

            //}

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
