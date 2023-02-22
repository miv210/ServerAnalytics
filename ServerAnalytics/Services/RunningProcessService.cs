using CsvHelper.Configuration;
using CsvHelper;
using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;


namespace ServerAnalytics.Services
{
    public class RunningProcessService : IRunningProcessesService
    {
        IRuntimeInformation runtimeInformation;
        public RunningProcessService(IRuntimeInformation runtimeInformation) 
        { 
            this.runtimeInformation = runtimeInformation;
        }

        //public RunningProcess GetRunnningProcesses()
        //{

        //}

        public List<RunningProcess> RunningOnWindows ()
        {

            var output = "";
            var info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.Arguments = $"/c chcp 65001 & tasklist /NH";
            info.RedirectStandardOutput = true;
            //info.StandardInputEncoding = System.Text.Encoding.UTF8;
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
