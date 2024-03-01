using WDS.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Linq;
using DataService.Models.Settings;
using DataService.Core.Settings;
using System;
using DataService.Models;
using DataService.Models.Test;
using WDS.Data;
using System.Data;

namespace WDS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TestController : WDSControllerBase
    {
        #region Members
        
        
        #endregion

        public TestController() : base("WDS")
        {
            
        }

        [HttpPut("TestAllApplications")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult TestAllApplications()
        {
            var returnValue = new Dictionary<string, List<TestModel>>();

            try
            {
                foreach (var application in DataServiceSettings.Applications.Values)
                {
                    returnValue.Add(application.Name, TestRecordset(application.Name)); 
                }
                
            }
            catch (Exception ex)
            {
                
            }

            return new ObjectResult(returnValue);
        }

        [HttpPut("TestRecordsets")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult TestRecordsets(string appName)
        {
            return new ObjectResult(TestRecordset(appName));
        }

        private List<TestModel> TestRecordset(string appName)
        {
            var returnValue = new List<TestModel>();

            if (DataServiceSettings.Applications.ContainsKey(appName))
            {
                try
                {
                    foreach (var rs in DataServiceSettings.Applications[appName].Recordsets.Values)
                    {
                        var allParameters = SQLHelpers.GetData<ParameterExampleValueModel>("[wds].[get_parameter_values]", "WDS", CommandType.StoredProcedure,
                                new Dictionary<string, string>() { { "appName", appName }, { "recordsetName", rs.Model.Name } });

                        if (allParameters != null && allParameters.Count > 0)
                        {
                            var setNames = allParameters.GroupBy(g => g.SetName).Select(o => o.First()).Select(o => o.SetName).ToList();

                            foreach (var setName in setNames)
                            {
                                var parameters = allParameters.Where(o => o.SetName == setName).ToDictionary(o => o.Name, o => o.Value);

                                var request = new WDSServerRequest(appName, rs.Model.Name, parameters, Request);
                                Processor.TryServiceRequest(request, out WDSServerResponse response);
                                returnValue.Add(new TestModel(response));
                            }
                        } else
                        {
                            // Try calling without any parameters
                            var parameters = new Dictionary<string, string>();

                            var request = new WDSServerRequest(appName, rs.Model.Name, parameters, Request);
                            Processor.TryServiceRequest(request, out WDSServerResponse response);
                            returnValue.Add(new TestModel(response));
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    
                }
            }

            return returnValue;
        }

    }
}
