using System.Data;
using DataService.Models.Settings;
using System;
using System.IO;
using Newtonsoft.Json;
using WDS.Core.Extensions;
using DataService.Core;
using DataService.Models;
using DataService.Core.Settings;
using WDSClient.Models;
using DataService.Core.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static WDSClient.Models.WDSEnums;

namespace WDS.Data
{
    public class WDSRequestProcessor
    {
        List<WDSProcessorWrapper> _callProcessor = new List<WDSProcessorWrapper>();

        #region Caching
        protected void SaveCachedVersion(WDSServerResponse response)
        {
            if (IsCached(response.Request, out string filePath)) return;

            var rs = DataServiceSettings.GetRecordsetModel(response.Request.Application, response.Request.Call);

            if (rs.IsCached)
            {
                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string jsonString = JsonConvert.SerializeObject(response);
                // file will be overwritten if it exists
                File.WriteAllText(filePath, AesOperation.EncryptString(WDSCache.CACHED_DATA_ENCRYPT_KEY, jsonString));
            }
        }

        private static WDSServerResponse GetCachedVersion(string filePath)
        {
            string rawData = AesOperation.DecryptString(WDSCache.CACHED_DATA_ENCRYPT_KEY, File.ReadAllText(filePath));
            var returnValue = JsonConvert.DeserializeObject<WDSServerResponse>(rawData);

            return returnValue;
        }

        private bool IsCached(WDSServerRequest request, out string filePath, bool forceUsingId = false)
        {
            var rs = DataServiceSettings.GetRecordsetModel(request.Application, request.Call);
            filePath = "";

            if (rs != null && rs.IsCached)
            {
                var fileHash = forceUsingId ? request.Id : AesOperation.EncryptString(WDSCache.CACHED_DATA_ENCRYPT_KEY, $"{request.CallParameters.ToString(";")}");
                filePath = $"{DataServiceSettings.Connections.ConnectionByName[WDSCache.WD_CACHE_ID]?.GetConnection()}\\{rs.Application}\\{fileHash}.{WDSCache.WD_CACHE_FILE_EXTENSION}";

                if (File.Exists(filePath))
                {
                    int timeout = rs.CacheTimeOut;
                    var fileDateTimeStamp = File.GetLastWriteTime(filePath);
                    if ((DateTime.Now - fileDateTimeStamp).TotalSeconds < timeout)
                        return true;
                }
            }

            return false;
        }
        #endregion

        public WDSRequestProcessor (List<WDSProcessorWrapper> callProcessor)
        {
            _callProcessor = callProcessor;
        }

        public int TryServiceRequest(WDSServerRequest request, out WDSServerResponse response)
        {
            var rs = DataServiceSettings.Rs(request.Application, request.Call);
            if (rs == null)
            {
                response = new WDSServerResponse(request) { ErrorMessage = $"Request for ({request.Application}.{request.Call}) was not found. RequestErrors:{request.ErrorMessage}" };
                return (int)HttpStatusCode.NotFound;
            }
            
            var recordsetRole = (rs == null) ? "" : rs.Model.Role;
            // TODO: Add user and session roles here. 
            if (!recordsetRole.IsNullOrEmpty() && !request.ApiKey.Roles.Contains(recordsetRole))
            {
                response = new WDSServerResponse(request) { ErrorMessage = $"Caller not authorized for ({request.Application}.{request.Call}). Please ensure the user/api-key has access to the role '{recordsetRole}'" };
                return (int)HttpStatusCode.Unauthorized;
            }

            if (!request.IsRequestValid() || request.ContainsError)
            {
                response = new WDSServerResponse(request) { ErrorMessage = $"Request for ({request.Application}.{request.Call}) is not valid. Please ensure the recordset exists and the parameters are correct. Error:{request.ErrorMessage} " };
                return (int)HttpStatusCode.BadRequest;
            }

            if (request.LogUserCallToDB())
                AddUserAccessLog(request);

            response = ProcessRequest(request); 
            if (response.ContainsError) return (int)HttpStatusCode.InternalServerError;
            return (int)HttpStatusCode.OK;
        }

        public async Task<WDSServerResponse> ProcessRequestAsync(WDSServerRequest request)
        {
            var response = ProcessRequest(request);
            return response; 
        }

        public async Task<List<WDSServerResponse>> ProcessRequestAsync(List<WDSServerRequest> requests)
        {
            var returnValue = new List<WDSServerResponse>();

            try
            {
                var rv = requests.Select(c => ProcessRequestAsync(c)).ToList();
                Task.WhenAll(rv);

                returnValue = rv.Select(o => o.Result).ToList();
            }
            catch (Exception ex)
            {
                var response = new WDSServerResponse();
                var message = $"Error occurred in WDSController.GetMany() - {ex.Message}";
                response.ErrorMessage = message;
                WDSLogs.WriteServiceDBLog(LogLevels.Error, message);
                returnValue.Add(response);
            }

            return returnValue;
        }
        public WDSServerResponse ProcessRequest(WDSServerRequest request)
        {
            WDSServerResponse returnValue;
            request.SetDataDetailTypeWithApplication();

            if (IsCached(request, out string filePath))
            {
                returnValue = GetCachedVersion(filePath);
                // Change any possible Deltas from the cached version

                if (returnValue.Request.RequestedDataFormatType != request.RequestedDataFormatType)
                {
                    // TODO: Transfrom the Return data into a possible new format
                }

                returnValue.RequestedDataDetailType = request.RequestedDataDetailType;
                return returnValue;
            }

            if (_callProcessor != null)
            {
                returnValue = ProcessCall(request);
                if (!returnValue.ContainsError)
                {
                    SaveCachedVersion(returnValue);
                }
                else
                {
                    var sub = request.GetSubstitute();
                    if (sub != null)
                        returnValue = ProcessRequest(sub);
                }
            }
            else
            {
                returnValue = new WDSServerResponse(request);
                returnValue.ErrorMessage = "The Call Processor is null. Request can not be evaluated. Contact System Adminitrator";
            }

            WDSLogs.AddLog(returnValue);
            return returnValue;
        }
        private WDSServerResponse ProcessCall(WDSServerRequest request)
        {
            var proc = _callProcessor.Where(o => o.Processor.SupportedProcessorType.Contains(request.CallType)).FirstOrDefault();

            if (proc != null)
                return proc.Processor.MakeCall(request);
            else
                return new WDSServerResponse(request) { ErrorMessage = $"{request.CallType} does not contain a mapped process at this time." };
        }
        private async void AddUserAccessLog(WDSServerRequest req)
        {
            //TODO: Write a change to the log table. 
            var parameters = new Dictionary<string, string>()
            {
                { "clientHost", req.Host },
                { "serverHost", DataServiceSettings.SystemEnvVariables["COMPUTERNAME"] },
                { "application", req.Application },
                { "recordset", req.Call },
                { "apiKey", req.ApiKey.Name },
                { "edipi", req.User == null ? 0.ToString(): req.User.EDIPI.ToString() },
                { "ptc", req.User == null ? "" : req.User.PTC.ToString() },
                { "parameters", req.CallParameters.ToString(";") }
            };

            var actualRequest = new WDSServerRequest("WDSLogging", "UserAccessLog", parameters, req.Host, req.User)
            {
                ApiKey = req.ApiKey                
            };
            actualRequest.SetToHasBeenLogged(); // To prevent recursive logs.  
            ProcessCall(actualRequest);

            req.SetToHasBeenLogged();
        }
    }
}
