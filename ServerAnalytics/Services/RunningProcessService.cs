﻿using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Collections.Immutable;
using System;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ServerAnalytics.Services
{
    public class RunningProcessService : IRunningProcessesService
    {
        ServerAnalyticsContext db;
        public void UpdateRunningProcesses ()
        {
            
            DateTime date = DateTime.UtcNow;
            Process[] processesList = Process.GetProcesses();



            var ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[2].ToString();

            Server server;
            using(db = new ServerAnalyticsContext())
            {
                server = db.Servers.FirstOrDefault(p => p.Ip == ip);
                if (server == null)
                {
                    Console.WriteLine("Сервера с таки адрессом не найденно");
                }
            }
          
            var runningProcesses = processesList.Select(p=> new RunningProcess
            {
                Name = p.ProcessName,
                PID = p.Id,
                SessionNumber = p.SessionId,
                Memory = p.WorkingSet / 1024,
                DateCheck = date,
                IdServer= server.Id,
                
            }).ToList();

            using (db = new ServerAnalyticsContext())
            {
                db.RunningProcesses.AddRange(runningProcesses);
                db.SaveChanges();
            }
        }

        public List<RunningProcess> GetRunningProcesses()
        {
            List<RunningProcess> runningProcesses = new List<RunningProcess>();


            List<Server> serverList;

            using(db = new ServerAnalyticsContext())
            {
                runningProcesses = db.RunningProcesses.ToList();
                serverList = db.Servers.ToList();
            }

            var withChildren = runningProcesses.GroupBy(p => new { p.Name, p.DateCheck} ).Select(p => new RunningProcess
            {
                Name = p.Key.Name,
                Memory = p.Sum(d => d.Memory / 1024),
                DateCheck = p.Key.DateCheck,
                Children = p.ToList(),
            }).ToList();

            var servers = serverList.GroupBy(p => new { p.Runnings, p.Ip, p.Name }).Select(p => new Server
            {
                Name = p.Key.Name,
                Ip = p.Key.Ip,
                Runnings= withChildren
            });

            foreach (var dsa in servers)
            {
                Console.WriteLine(dsa.Name + "\n");
                foreach (var lk in dsa.Runnings)
                {
                    Console.WriteLine($"{lk.Name}");
                }
            }
            return withChildren;
        }

        public List<RunningProcess> RunningOnWindowsCMD()
        {
            var output = "";
            var info = new ProcessStartInfo();

            info.FileName = "cmd.exe";
            info.Arguments = $"/c chcp 65001 & tasklist /NH";
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.StandardOutputEncoding = System.Text.Encoding.UTF8;

            List<RunningProcess> records = new List<RunningProcess>();
            RunningProcess runningProcess;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < lines.Length; i++)
            {
                var allline = lines[i].Split("  ", StringSplitOptions.RemoveEmptyEntries);
                var pidNameSe = allline[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var memory = allline[3].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                runningProcess = new RunningProcess
                {
                    Name = allline[0],
                    PID = Convert.ToInt32(pidNameSe[0]),
                    NameSession = pidNameSe[1],
                    SessionNumber = Convert.ToInt32(allline[2]),
                    Memory = Convert.ToInt32(memory[0]),
                    DateCheck = DateTime.UtcNow,
                };
                //using()
                //runningProcess.Name = allline[0];
                //runningProcess.PID = Convert.ToInt32(pidNameSe[0]);
                //runningProcess.NameSession = pidNameSe[1];
                //runningProcess.SessionNumber = Convert.ToInt32(allline[2]);
                //runningProcess.Memory = allline[3];
                //runningProcess.DateChek = DateTime.UtcNow;

                records.Add(runningProcess);
            }


            return records;
        }
    }
}
