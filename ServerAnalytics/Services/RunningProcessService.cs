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
        public List<RunningProcess> GetRunningProcesses ()
        {
            List<RunningProcess> records = new List<RunningProcess>();
            RunningProcess runningProcess;

            Process[] processesList = Process.GetProcesses();
            
            
            foreach ( Process process in processesList )
            {
                runningProcess = new RunningProcess();

                runningProcess.Name = process.ProcessName;
                runningProcess.PID = process.Id;
                runningProcess.SessionNumber = process.SessionId;
                runningProcess.Memory = $"{process.WorkingSet /1024} K";
                runningProcess.DateChek = DateTimeOffset.Now;
                records.Add(runningProcess);
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
                runningProcess = new RunningProcess();
                var allline = lines[i].Split("  ", StringSplitOptions.RemoveEmptyEntries);
                var pidNameSe = allline[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                runningProcess.Name = allline[0];
                runningProcess.PID = Convert.ToInt32(pidNameSe[0]);
                runningProcess.NameSession = pidNameSe[1];
                runningProcess.SessionNumber = Convert.ToInt32(allline[2]);
                runningProcess.Memory = allline[3];
                runningProcess.DateChek = DateTimeOffset.Now;

                records.Add(runningProcess);
            }
            return records;
        }
    }
}
