using BlackHole.Server.Hubs;
using BlackHole.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlackHole.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : ControllerBase
    {
        public DownloadController(IHubContext<ServiceQueueHub> queueService, ServiceControl serviceControl)
        {
            _queueService = queueService;
            _serviceControl = serviceControl;
        }

        private readonly ServiceControl _serviceControl;
        private readonly IHubContext<ServiceQueueHub> _queueService;

        [HttpPost]
        public async Task<IActionResult> NewDownload(DownloadData download)
        {
            bool result = _serviceControl.Add(download);

            if (result)
            {
                await _queueService.QueueAdd(download);
                return Ok();
            }

            return Conflict();
        }

        [HttpGet]
        public IActionResult GetDownloads()
        {
            return Ok(_serviceControl.GetQueueByType<DownloadData>());
        }

        [HttpGet]
        [Route("{id:guid}/{filename}")]
        public IActionResult GetDownload(Guid id, string filename)
        {
            var downloadData = _serviceControl.Get<DownloadData>(id);

            if (downloadData == null)
                return NotFound();

            if (System.IO.File.Exists(downloadData.PathDestination))
                return File(System.IO.File.ReadAllBytes(downloadData.PathDestination), "application/octet-stream", downloadData.FileName);

            return NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteDownload(Guid id)
        {
            var downloadData = _serviceControl.Get<DownloadData>(id);

            if (downloadData == null)
                return NotFound();

            if (downloadData.IsProcessing)
                return Conflict();


            if (System.IO.File.Exists(downloadData.PathDestination))
                System.IO.File.Delete(downloadData.PathDestination);

            bool result = _serviceControl.Remove(downloadData);

            if (result)
            {
                await _queueService.QueueRemoved(id);
                return Ok();
            }

            return NotFound();
        }
    }
}
