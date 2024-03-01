using WDS.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Linq;
using DataService.Models.Settings;
using DataService.Core.Settings;

namespace WDS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class WDSController : WDSControllerBase
    {
        #region Members
        
        
        #endregion

        public WDSController() : base("WDS")
        {
            
        }

        [HttpPut("GetApplicationData")]
        [Authorize(Policy = Policies.Read)]
        public IActionResult GetApplicationData(string application, string call, Dictionary<string, string> parameters)
        {
            var request = new WDSServerRequest(application, call, parameters, Request);

            return MakeCallForObjectResult(request);
        }

        [HttpPut("GetApplications")]
        [Authorize(Policy = Policies.Read)]
        public IActionResult GetApplications()
        {
            return new ObjectResult(DataServiceSettings.Applications.Keys.ToList());
        }

        [HttpPut("GetRecordsets")]
        [Authorize(Policy = Policies.Read)]
        public IActionResult GetRecordsets(string application)
        {
            var returnValue = new ObjectResult(null);

            if (DataServiceSettings.Applications.ContainsKey(application))
                returnValue = new ObjectResult(DataServiceSettings.Applications[application].Recordsets.Keys.ToList());

            return returnValue;
        }

    }
}
