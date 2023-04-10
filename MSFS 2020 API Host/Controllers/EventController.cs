using Microsoft.AspNetCore.Mvc;
using static JohnPenny.MSFS.SimConnectManager.REST.Controllers.AircraftController;

namespace JohnPenny.MSFS.SimConnectManager.REST.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class EventController : Controller
    {
        public class Event
        {
            public string Name { get; set; }
        }

        [HttpPost]
        [Route("/[controller]")]
        public IActionResult Set([FromBody] Event eventParam)
        {
            if (!Program.simConnectManager.SetUp()) return BadRequest(ErrorCode.CannotReadSimObjectWithoutConnection.ToString()); // setup failed, try again and report
            Program.simConnectManager.SendMobiFlightEvent(eventParam.Name);
            return Ok();
        }
    }
}
