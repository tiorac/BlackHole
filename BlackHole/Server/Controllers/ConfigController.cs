using BlackHole.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BlackHole.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        [HttpGet("system")]
        public ActionResult<Dictionary<string, string>> ViewSystem()
        {
            var config = new Dictionary<string, string>
            {
                { "Version", Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString() ?? "" },
                { "OS", Environment.OSVersion.ToString() },
                { "Machine Name", Environment.MachineName },
                { "Config Folder", ConfigurationHelper.GetConfigFolder() },
                { "DB File", BaseRepository.GetFileName() }
            };

            return config;
        }

    }
}
