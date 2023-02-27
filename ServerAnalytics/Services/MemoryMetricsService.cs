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
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput= true;

            using(var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split('\n');
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetric();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = (double)(metrics.Total - metrics.Free);
            metrics.DateCheck = DateTime.UtcNow;
            List<MemoryMetric> memoryMetrics = new List<MemoryMetric>();

            using(db = new ServerAnalyticsContext())
            {
                db.MemoryMetrics.Add(metrics);
                db.SaveChanges();
            }
            using (db = new ServerAnalyticsContext())
            {
                memoryMetrics = db.MemoryMetrics.ToList();
            }
            return memoryMetrics;
        }

        private List<MemoryMetric> GetUnixMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo("free -m");
            info.FileName = "/bin/bash";
            info.Arguments = "-c \"free -m\"";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }

            var lines = output.Split("\n");
            var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetric();
            metrics.Total = double.Parse(memory[1]);
            metrics.Used = double.Parse(memory[2]);
            metrics.Free = double.Parse(memory[3]);
            metrics.DateCheck = DateTime.UtcNow;

            List<MemoryMetric> memoryMetrics = new List<MemoryMetric>();

            using (db = new ServerAnalyticsContext())
            {
                db.MemoryMetrics.Add(metrics);
                db.SaveChanges();
            }
            using (db = new ServerAnalyticsContext())
            {
                memoryMetrics = db.MemoryMetrics.ToList();
            }
            return memoryMetrics;
        }
    }
}
