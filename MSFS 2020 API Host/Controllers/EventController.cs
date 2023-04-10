using Microsoft.AspNetCore.Mvc;
using Microsoft.FlightSimulator.SimConnect;
using static JohnPenny.MSFS.SimConnectManager.REST.Controllers.AircraftController;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using Microsoft.Extensions.Logging;
using MobiFlight.SimConnectMSFS;
using System.Runtime.InteropServices;

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

        static bool initialized = false;

        [HttpPost]
        [Route("/[controller]")]
        public IActionResult Set([FromBody] Event eventParam)
        {
            if (!Program.simConnectManager.SetUp()) return BadRequest(ErrorCode.ItemStructureInvalid.ToString()); // setup failed, try again and report
            if(initialized == false)
            {
                initialized = true;
                InitializeClientDataAreas(Program.simConnectManager.simConnect);
            }

            WasmModuleClient.SendWasmCmd(Program.simConnectManager.simConnect, "MF.SimVars.Set." + eventParam.Name);
            WasmModuleClient.DummyCommand(Program.simConnectManager.simConnect);
            return Ok();
        }

        private const string MOBIFLIGHT_CLIENT_DATA_NAME_SIMVAR = "MobiFlight.LVars";
        private const string MOBIFLIGHT_CLIENT_DATA_NAME_COMMAND = "MobiFlight.Command";
        private const string MOBIFLIGHT_CLIENT_DATA_NAME_RESPONSE = "MobiFlight.Response";
        private const int MOBIFLIGHT_MESSAGE_SIZE = 1024;

        private void InitializeClientDataAreas(SimConnect sender)
        {
            // register Client Data (for SimVars)
            (sender).MapClientDataNameToID(MOBIFLIGHT_CLIENT_DATA_NAME_SIMVAR, SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_LVARS);
            (sender).CreateClientData(SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_LVARS, 4096, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

            // register Client Data (for WASM Module Commands)
            var ClientDataStringSize = (uint)Marshal.SizeOf(typeof(ClientDataString));
            (sender).MapClientDataNameToID(MOBIFLIGHT_CLIENT_DATA_NAME_COMMAND, SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_CMD);
            (sender).CreateClientData(SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_CMD, MOBIFLIGHT_MESSAGE_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

            // register Client Data (for WASM Module Responses)
            (sender).MapClientDataNameToID(MOBIFLIGHT_CLIENT_DATA_NAME_RESPONSE, SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_RESPONSE);
            (sender).CreateClientData(SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_RESPONSE, MOBIFLIGHT_MESSAGE_SIZE, SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

            (sender).AddToClientDataDefinition((SIMCONNECT_DEFINE_ID)0, 0, MOBIFLIGHT_MESSAGE_SIZE, 0, 0);
            (sender).RegisterStruct<SIMCONNECT_RECV_CLIENT_DATA, ResponseString>((SIMCONNECT_DEFINE_ID)0);
            (sender).RequestClientData(
                SIMCONNECT_CLIENT_DATA_ID.MOBIFLIGHT_RESPONSE,
                (SIMCONNECT_REQUEST_ID)0,
                (SIMCONNECT_DEFINE_ID)0,
                SIMCONNECT_CLIENT_DATA_PERIOD.ON_SET,
                SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.CHANGED,
                0,
                0,
                0
            );
        }
    }
}
