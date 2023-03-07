using ServerAnalyticsApp.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using System.Text;

namespace ServerAnalyticsApp.Services
{
    public class RunningProcessService
    {
        public async Task UpdateRunningProcesses()
        {

            DateTime date = DateTime.UtcNow;
            Process[] processesList = Process.GetProcesses();
            Server[] servers;

            var ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[2].ToString();

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
                            var runningProcesses = processesList.Select(p => new RunningProcess
                            {

                                Name = p.ProcessName,
                                PID = p.Id,
                                SessionNumber = p.SessionId,
                                Memory = p.WorkingSet / 1024,
                                DateCheck = date,
                                IdServer = server.Id,

                            }).ToList();

                            try
                            {
                                var requestUri = "http://localhost:5272/api/metrics/update/runnong_processes";

                                string json = JsonConvert.SerializeObject(runningProcesses);
                                var content = new StringContent(json, Encoding.UTF8, "application/json");
                                //Console.WriteLine(Convert.ToString(json));
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

        //using(db = new ServerAnalyticsContext())
        //{
        //    server = db.Servers.FirstOrDefault(p => p.Ip == ip);
        //    if (server == null)
        //    {
        //        Console.WriteLine("Сервера с таки адрессом не найденно");
        //    }
        //}

        //var runningProcesses = processesList.Select(p=> new RunningProcess
        //{
        //    Name = p.ProcessName,
        //    PID = p.Id,
        //    SessionNumber = p.SessionId,
        //    Memory = p.WorkingSet / 1024,
        //    DateCheck = date,


        //}).ToList();

        //using (db = new ServerAnalyticsContext())
        //{
        //    db.RunningProcesses.AddRange(runningProcesses);
        //    db.SaveChanges();
        //}

    }
}
