using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IRunningProcessesService
    {
        List<RunningProcess> GetRunningProcesses();
        void UpdateRunningProcesses(List<RunningProcess> runningProcesses);
    }
}
