using WDS.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using WDSClient.Models;
using DataService.Core.Settings;
using DataService.Models.Settings;
using static WDSClient.Models.WDSEnums;

namespace WDS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class LogController : WDSControllerBase
    {
        private const string APPLICATION_NAME = "WDSLogging";
        public LogController() : base(APPLICATION_NAME)
        {

        }

        [HttpPut("Log")]
        [Authorize(Policy = Policies.Create)]
        public IActionResult Log(string clientHost, string application, string area, string message, LogLevels logLevel)
        {
            var dsa = DataServiceSettings.Applications; 
            var parameters = new Dictionary<string, string>()
            {
                { "ClientHost", clientHost },                
                { "application", application },
                { "area", area },
                { "message", message },
                { "logLevel", ((int)logLevel).ToString() }
            };
            var request = new WDSServerRequest(APPLICATION_NAME, "ApplicationLog", parameters, Request);

            return MakeCallForObjectResult(request);
        }
    }
}
