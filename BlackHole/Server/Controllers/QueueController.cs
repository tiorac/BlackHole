using BlackHole.Server.Hubs;
using BlackHole.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace BlackHole.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        public QueueController(IHubContext<ServiceQueueHub> queueService, ServiceControl serviceControl)
        {
            _queueService = queueService;
            _serviceControl = serviceControl;
        }

        private readonly IHubContext<ServiceQueueHub> _queueService;
        private readonly ServiceControl _serviceControl;
        private static ConcurrentBag<StreamWriter> _clients = new();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_serviceControl.Queue);
        }


    }
}
