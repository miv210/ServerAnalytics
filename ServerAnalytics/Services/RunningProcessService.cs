using ServerAnalytics.Models;
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
        public void UpdateRunningProcesses (List<RunningProcess> runningProcesses)
        {
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

            var withChildren = runningProcesses.GroupBy(p => new { p.Name, p.DateCheck, p.IdServer} ).Select(p => new RunningProcess
            {
                

                Name = p.Key.Name,
                Memory = p.Sum(d => d.Memory / 1024),
                DateCheck = p.Key.DateCheck,
                IdServer = p.Key.IdServer,
                Date = p.Key.DateCheck.ToString("d.M.yyyy HH:mm"),
                Children = p.ToList(),
            }).ToList();

            return withChildren;
        }
    }
}
