using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataService.Models.Settings;
using DataService.Core.Logging;
using System.Threading.Tasks;
using WDS.Core.Extensions;
using DataService.Models;
using System.IO;
using System.Text;
using WDS.Authentication;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System;
using WDS.Authorization;
using DataService.Core.Settings;
using WDS.Models;
using WDS.Data;
using static WDSClient.Models.WDSEnums;

namespace WDS.Controllers
{
    /// <summary>
    /// Base controller class for the WDS. It handles all the base functionality that all the other controllers depends on. 
    /// </summary>
    public abstract class WDSControllerBase : ControllerBase
    {
        #region Properties
        public readonly string APPLICATION_NAME;
        protected List<WDSProcessorWrapper> RequestProcessors = new List<WDSProcessorWrapper>();
        public WDSRequestProcessor Processor = null; 

        private Application _applicationSettings;
        internal ApiKeyModel ApiKey
        {
            get
            {
                Request.Headers.TryGetValue(HttpHeaderNameConstants.ApiKey, out var returnValue);

                return DataServiceSettings.ApiKeys.RetrieveKey(returnValue); 
            }
        }
        
        private string _requestBody;
        internal string RequestBody
        {
            get
            {
                if (_requestBody.IsNullOrEmpty() && Request != null)
                {
                    using (StreamReader sr = new StreamReader(Request.Body, Encoding.UTF8, true, 1024, true))
                    {
                        Task<string> t;
                        t = sr.ReadToEndAsync();
                        _requestBody = t.Result;
                    }
                }
                return _requestBody;
            }
        }
        #endregion

        public WDSControllerBase(string application)
        {
            APPLICATION_NAME = application;

            if (DataServiceSettings.Applications.ContainsKey(application))
            {
                _applicationSettings = DataServiceSettings.Applications[application];                
            }

            RequestProcessors.Add(new WDSProcessorWrapper(new WDSSQLProcessorHandler())); 
            RequestProcessors.Add(new WDSProcessorWrapper(new WDSFileDataProcessorHandler())); 
            RequestProcessors.Add(new WDSProcessorWrapper(new WDSDailyFileUpdateProcessorHandler())); 
            RequestProcessors.Add(new WDSProcessorWrapper(new WDSFileUpdateProcessorHandler()));
            Processor = new WDSRequestProcessor(RequestProcessors);
        }

        #region Global Endpoints
        [HttpPost("GetMulti")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public IActionResult GetMulti(List<WDSServerRequest> req)
        {
            // This function must return a list of WDSServerResponse objects regardless of any error messages within

            ObjectResult returnValue = null;
            if (req == null) return returnValue;

            req.ForEach(o => new WDSServerRequest(o, Request));
            req.ForEach(o =>
                {
                    if (o.Application.IsNullOrEmpty())
                        o.Application = APPLICATION_NAME;
                    o.ApiKey = ApiKey;
                });
            req = req.Where(c => !c.NeedsEndpoint).ToList();
            

            try
            {
                var rv = req.Select(c => MakeCallAsync(c, true)).ToList();
                Task.WhenAll(rv);

                returnValue = new ObjectResult(rv.Select(o => o.Result.Value));
            }
            catch (Exception ex)
            {
                var response = new WDSServerResponse();
                var message = $"Error occurred in WDSController.GetMany() - {ex.Message}"; 
                response.ErrorMessage = message;
                WDSLogs.WriteServiceDBLog(LogLevels.Error, message);
                returnValue = new ObjectResult(response);
            }

            
            return returnValue;
        }

        [HttpPost("GetData")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public IActionResult GetData(string call, Dictionary<string, string> parameters)
        {
            WDSServerRequest request = null;
            var seperator = call.IndexOf('.');

            if (seperator > 0)
                request = new WDSServerRequest(call.Substring(0, seperator), call.Substring(seperator + 1), parameters, Request);
            else
                request = new WDSServerRequest(APPLICATION_NAME, call, parameters, Request);

            return MakeCallForObjectResult(request);
        }

        [HttpPost("GetRawData"), Produces("text/plain")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public IActionResult GetRawData(string call, Dictionary<string, string> parameters)
        {
            WDSServerRequest request = null;
            var seperator = call.IndexOf('.');

            if (seperator > 0)
                request = new WDSServerRequest(call.Substring(0, seperator), call.Substring(seperator + 1), parameters, Request);
            else
                request = new WDSServerRequest(APPLICATION_NAME, call, parameters, Request);

            request.RequestedDataDetailType = DataDetailTypes.Simple; 

            return MakeCallForObjectResult(request);
        }

        #endregion

        #region Helper Functions
        public async Task<ObjectResult> MakeCallAsync(WDSServerRequest request, bool wrapErrors = false)
        {
            return MakeCallForObjectResult(request, wrapErrors);
        }
        public ObjectResult MakeCallForObjectResult(WDSServerRequest request, bool wrapErrors = false)
        {
            var statusCode = Processor.TryServiceRequest(request, out WDSServerResponse response); 

            if (statusCode != (int)HttpStatusCode.OK)
                return IfWrapErrorInWDSContract(StatusCode(statusCode, response.ErrorMessage), wrapErrors);
            //response.Request.Sanitize();

            var returnValue = new ObjectResult(response.GetHttpResponse()); 
            return returnValue;
        }   

        // Used in the case of multiple calls in which some need to return an error. 
        private ObjectResult IfWrapErrorInWDSContract(ObjectResult error, bool wrapError)
        {
            if (wrapError)
                return new ObjectResult(new WDSResponseContract() { ErrorMessage = $"{error.StatusCode}-{error.Value}" });
            else
                return error; 
        }
        
        #endregion
    }
}
