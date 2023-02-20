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
        public MetricsController(IMemoryMetricsService memoryMetricsService) 
        {
            this.memoryMetricsService = memoryMetricsService;
        }
        [HttpGet("")]
        public async Task<ActionResult<MemoryMetric>> GetMemory() 
        {
            var metric = memoryMetricsService.GetMemoryMetric();

            return new JsonResult(metric);
        }
    }
}
