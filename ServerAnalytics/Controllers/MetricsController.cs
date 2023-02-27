using Microsoft.AspNetCore.Mvc;
using ServerAnalytics.Models;
using ServerAnalytics.Services;
using ServerAnalytics.Services.Interface;

namespace ServerAnalytics.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class MetricsController : Controller
    {
        IMemoryMetricsService memoryMetricsService;
        IRunningProcessesService runningProcessesService;
        
        public MetricsController(IMemoryMetricsService memoryMetricsService, IRunningProcessesService runningProcessesService) 
        {
            this.memoryMetricsService = memoryMetricsService;
            this.runningProcessesService = runningProcessesService;
        }
        [HttpGet("/afdadfas")]
        public async Task<ActionResult<MemoryMetric>> GetMemory() 
        {
            var metric = memoryMetricsService.GetMemoryMetric();

            return new JsonResult(metric);
        }

        [HttpGet("")]
        public async Task<ActionResult<List<RunningProcess>>> GetRunningProcesses()
        {
            var ds = runningProcessesService.RunningOnWindows();
            //runningProcessesService.RunningOnWindows();
            return new JsonResult(ds);
        }
    }
}
