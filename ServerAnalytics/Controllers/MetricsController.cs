using Microsoft.AspNetCore.Mvc;
using ServerAnalytics.Models;
using ServerAnalytics.Services;
using ServerAnalytics.Services.Interface;
using System.IdentityModel.Tokens.Jwt;

namespace ServerAnalytics.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class MetricsController : Controller
    {
        IMemoryMetricsService memoryMetricsService;
        IRunningProcessesService runningProcessesService;
        IProcessorMetricsService processorMetricsService;
        IAuthService authService;

        public MetricsController(
            IMemoryMetricsService memoryMetricsService, 
            IRunningProcessesService runningProcessesService, 
            IProcessorMetricsService processorMetricsService,
            IAuthService authService
            
            ) 
        {
            this.authService= authService;
            this.memoryMetricsService = memoryMetricsService;
            this.runningProcessesService = runningProcessesService;
            this.processorMetricsService = processorMetricsService;
        }


        [HttpPost("login/{user}")]
        public async Task<ActionResult> GenerationJWT(User user)
        {

            var jwt = authService.GenerationJWT(user);
            return jwt;
        }

        [HttpPost("update/runnong_processes")]
        public async Task<ActionResult> UpdateRunningMetrics([FromBody]List<RunningProcess> runningProcesses)
        {
            runningProcessesService.UpdateRunningProcesses(runningProcesses);
            return new OkResult();
        }

        [HttpPost("update/memory")]
        public async Task<ActionResult> UpdateMemoryMetrics([FromBody]MemoryMetric memoryMetric)
        {
            try
            {
                memoryMetricsService.UpdateMemoryMetrics(memoryMetric);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new OkResult();
        }

        [HttpGet("memory")]
        public async Task<ActionResult<List<MemoryMetric>>> GetMemory() 
        {
            var metric = memoryMetricsService.GetMemoryMetric();

            return new JsonResult(metric);
        }

        [HttpGet("runninProcesses")]
        public async Task<ActionResult<List<RunningProcess>>> GetRunningProcesses()
        {
            var metric = runningProcessesService.GetRunningProcesses();
            return new JsonResult(metric);
        }

        [HttpGet("processor")]
        public async Task<ActionResult<Task<WorkLodaProcessor>>> GetMetricsProcessor()
        {
            var metric = processorMetricsService.GetCpuMetrics();
            return new JsonResult(metric);
        }
    }
}
