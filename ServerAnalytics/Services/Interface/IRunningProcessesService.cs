using ServerAnalytics.Models;

namespace ServerAnalytics.Services.Interface
{
    public interface IRunningProcessesService
    {
        //RunningProcess GetRunnningProcesses();
        List<RunningProcess> RunningOnWindows();
    }
}
