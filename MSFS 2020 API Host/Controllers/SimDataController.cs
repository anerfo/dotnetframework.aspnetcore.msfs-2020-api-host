using JohnPenny.MSFS.SimConnectManager.REST.Interfaces;
using JohnPenny.MSFS.SimConnectManager.REST.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FlightSimulator.SimConnect;
using System.Collections.Generic;
using static JohnPenny.MSFS.SimConnectManager.REST.Controllers.AircraftController;
using static JohnPenny.MSFS.SimConnectManager.SimConnectManager;

namespace JohnPenny.MSFS.SimConnectManager.REST.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class SimDataController : ControllerBase
    {
        public SimDataController(IDataRepository<SimData> repository)
        {
            _dataRepository = repository;
        }

        public readonly IDataRepository<SimData> _dataRepository;

        [HttpGet]
        [Route("/[controller]/{i?}")]
        public IActionResult HandleURI(string id)
        {
            var result = new Dictionary<string, string>();
            foreach(var value in _dataRepository.All)
            {
                result[value.Name] = value.Value;
            }
            Program.simConnectManager.RequestSimData(RequestName.IdeaAircraftNearMe, id);
            return Ok(result);
        }

        [HttpPost]
        [Route("/[controller]/{i?}")]
        public IActionResult HandleURI(string id, [FromBody] List<string> variables)
        {
            if (!Program.simConnectManager.SetUp()) return BadRequest(ErrorCode.CannotReadSimObjectWithoutConnection.ToString());
            Program.simConnectManager.AddDataDefinition(id, variables, Callback);
            return Ok();
        }

        private void Callback(Dictionary<string, string> data)
        {
            foreach(var value in data)
            {
                _dataRepository.Update(new SimData { Name = value.Key, Value = value.Value });
            }
        }
    }
}
