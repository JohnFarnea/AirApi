using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AirApi;

namespace AirApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirController : ControllerBase
    {
        private ILogger _logger;
        private Api _air;
        private IConfiguration _config;
        public AirController(ILogger<AirController> logger, Api air, IConfiguration config)
        {
            _logger = logger;
            _air = air;
            _config = config;
        }

        [HttpGet("[action]")]
        public ActionResult<AirStatus> Status()
        {
            _logger.LogInformation("Getting Status");
            var result = _air.GetStatus();
            _logger.LogInformation("Got Status-On : {result}", result.On);
            return result;
        }

        [HttpGet("[action]/{number}")]
        public ActionResult<AirZoneStatus> Zone(int number)
        {
            _logger.LogInformation("Getting Zone Status : {number}",number);
            var result = _air.GetZoneStatus(number);
            _logger.LogInformation("Zone {number} Status Result : {result}", number, result.ZoneStatusString);
            return result;
        }

        [HttpGet("[action]/{state}")]
        public ActionResult<string> SetPower(int state)
        {
            string result = _air.SetPower(state);
            return result;
        }

        [HttpGet("[action]/{zone}/{state}")]
        public ActionResult<string> SetZoneState(int zone, AirZoneStateType state)
        {
            string result = _air.SetZone(zone, state);
            return result;
        }
    }
}

