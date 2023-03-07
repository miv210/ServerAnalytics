
using Newtonsoft.Json;
using ServerAnalyticsApp.Models;
using System.Diagnostics;
using ServerAnalyticsApp.Models;
using System.Net.Http.Json;
using System.Net;
using System.Text;

namespace ServerAnalyticsApp.Services
{
    public class MemoryMetricsService 
    {
        RuntimeInformationService runtimeInformation = new RuntimeInformationService();
        
        public MemoryMetric UpdateMemoryMetrics(Server server)
        {
            if (runtimeInformation.IsUnix())
            {
                return UpdateUnixMetrics(server);
            }
            return UpdateWindowsMetrics(server);
        }
      
        private MemoryMetric UpdateWindowsMetrics(Server server)
        {
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
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
            metrics.IdServer= server.Id;
            return metrics;
        }

        private MemoryMetric UpdateUnixMetrics(Server server)
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
            metrics.IdServer = server.Id;
            return metrics;
        }
        public async Task Update()
        {
            var ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[2].ToString();
            Server[] servers;
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync("http://localhost:5272/api/servers");
                    if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound)
                    {
                        Error? error = await response.Content.ReadFromJsonAsync<Error>();
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(error?.Message);
                    }
                    else
                    {
                        servers = await response.Content.ReadFromJsonAsync<Server[]>();
                        var server = servers.FirstOrDefault(p => p.Ip == ip);
                        if (server != null)
                        {
                            MemoryMetric memoryMetric = UpdateMemoryMetrics(server);
                            try
                            {
                                var requestUri = "http://localhost:5272/api/metrics/update/memory";

                                string json = JsonConvert.SerializeObject(memoryMetric);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                Console.WriteLine(Convert.ToString(json));
                                var pure = await client.PostAsync(requestUri, content);
                                Console.WriteLine($"Обновленно {pure.StatusCode}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Сервер {ip} не найден");

                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
