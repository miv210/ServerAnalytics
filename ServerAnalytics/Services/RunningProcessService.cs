﻿using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Collections.Immutable;
using System;
using Microsoft.EntityFrameworkCore;

namespace ServerAnalytics.Services
{
    public class RunningProcessService : IRunningProcessesService
    {
        ServerAnalyticsContext db;
        public void UpdateRunningProcesses ()
        {
            RunningProcess runningProcess;
            DateTime date = DateTime.UtcNow;
            Process[] processesList = Process.GetProcesses();
            
            var runningProcesses = processesList.Select(p=> new RunningProcess
            {
                Name = p.ProcessName,
                PID = p.Id,
                SessionNumber = p.SessionId,
                Memory = p.WorkingSet / 1024,
                DateCheck = date
            }).ToList();

            using (db = new ServerAnalyticsContext())
            {
                db.RunningProcesses.AddRange(runningProcesses);
                db.SaveChanges();
            }
            //var parent = processesList.GroupBy(p => p.ProcessName).Select(p => new RunningProcess
            //{
            //    Name = p.Key,
            //    Memory = p.Sum(d => d.WorkingSet / 1024),
            //    DateCheck = date,
            //    Children = p.Select(d => new RunningProcess
            //    {
            //        Name = d.ProcessName,
            //        PID = d.Id,
            //        SessionNumber = d.SessionId,
            //        Memory = d.WorkingSet / 1024,
            //        DateCheck = date
            //    }).ToList(),
            //});
        }

        public List<RunningProcess> GetRunningProcesses()
        {
            List<RunningProcess> runningProcesses = new List<RunningProcess>();

            using(db = new ServerAnalyticsContext())
            {
                runningProcesses = db.RunningProcesses.ToList();
            }

            var withChildren = runningProcesses.GroupBy(p => new { p.Name, p.DateCheck, p.Children } ).Select(p => new RunningProcess
            {
                Name = p.Key.Name,
                Memory = p.Sum(d => d.Memory / 1024),
                DateCheck = p.Key.DateCheck,
                Children = runningProcesses.Where(d=> d.Name == p.Key.Name ).ToList(),
            }).ToList();

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
