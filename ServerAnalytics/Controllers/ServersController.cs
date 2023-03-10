using Microsoft.AspNetCore.Mvc;
using ServerAnalytics.Models;
using ServerAnalytics.Services.Interface;

namespace ServerAnalytics.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ServersController : Controller
    {
        IServerService serverService;
        public ServersController(IServerService serverService)
        {
            this.serverService = serverService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<Server>>> GetAll()
        {
            var serverList = serverService.GetAll();
            return new JsonResult(serverList);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> Get(int id)
        {
            var getServer = serverService.Get(id);
            return new JsonResult(getServer);
        }
        [HttpPut("")]
        public async Task<ActionResult> Update(Server server)
        {
            serverService.Update(server);
            return new OkResult();
        }
        [HttpPost("")]
        public async Task<ActionResult> AddServer(Server server)
        {
            serverService.Add(server);
            return new OkResult();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            serverService.Delete(id);
            return new OkResult();
        }
    }
}
