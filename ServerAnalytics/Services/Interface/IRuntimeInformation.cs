using System.Runtime.InteropServices;

namespace ServerAnalytics.Services.Interface
{
    public interface IRuntimeInformation
    {
        public OSPlatform GetOSPlatform();
        public bool IsUnix();
        public bool IsFreeBSD();
        public bool IsOSX();
        public bool IsLinux();
        public bool IsWindows();
    }
}
