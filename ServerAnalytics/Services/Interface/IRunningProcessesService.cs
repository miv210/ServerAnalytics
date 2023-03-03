using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IRunningProcessesService
    {
        List<RunningProcess> GetRunningProcesses();
        void UpdateRunningProcesses(string nameMachine = "DESKTOP-K9EKI4B");
    }
}
