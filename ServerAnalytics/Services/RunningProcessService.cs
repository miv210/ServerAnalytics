using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Collections.Immutable;

namespace ServerAnalytics.Services
{
    public class RunningProcessService : IRunningProcessesService
    {
        ServerAnalyticsContext db;
        public List<RunningProcess> GetRunningProcesses ()
        {
            List<RunningProcess> records = new List<RunningProcess>();
            RunningProcess runningProcess;

            Process[] processesList = Process.GetProcesses();
            
            
            foreach ( Process process in processesList )
            {
                runningProcess = new RunningProcess
                {
                    
                    Name = process.ProcessName,
                    PID = process.Id,
                    SessionNumber = process.SessionId,
                    Memory = process.WorkingSet / 1024,
                    DateCheck = DateTime.UtcNow
                };
                records.Add( runningProcess );
            }
            using (db = new ServerAnalyticsContext())
            {
                db.RunningProcesses.AddRange( records );
                db.SaveChanges();
                records = db.RunningProcesses.ToList();
            }
            return records;
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
