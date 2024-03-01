using DataService.Core;
using DataService.Core.Settings;
using DataService.Models;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mime;
using WDS.Authorization;
using WDS.Controllers;
using WDS.Core.Extensions;
using WDS.Settings;

namespace DataService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AdminController : WDSControllerBase
    {
        public AdminController() : base("Admin")
        {
           
        }

        #region API Key Stuff
        [HttpGet("Encrypt")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Read")]
        public IActionResult Encrypt(string salt, string plainText)
        {
            return new ObjectResult(AesOperation.EncryptString(salt, plainText));
        }

        [HttpGet("Decrypt")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult Decrypt(string salt, string cipherText)
        {
            return new ObjectResult(AesOperation.DecryptString(salt, cipherText));
        }

        [HttpGet("GenerateGuid")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Read")]
        public IActionResult GenerateGuid()
        {
            return new ObjectResult(Guid.NewGuid());
        }

        [HttpPost("CreateAPIKey")]
        [Authorize(Policy = Policies.Full,Roles = "WDS.Admin")]
        public IActionResult CreateAPIKey(string name, string ownerEmail, string ipMask, string description)
        {
            var guid = DataServiceSettings.ApiKeys.AddAPIKey(name, ownerEmail, ipMask, description);
            
            return new ObjectResult(guid);
        }

        [HttpGet("GetAPIKey")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult GetAPIKey(string keyName)
        {
            var returnValue = DataServiceSettings.ApiKeys.RetrieveKeyByName(keyName); 

            if (returnValue == null || returnValue.ApiKey.IsNullOrEmpty())
            {
                var request = new WDSServerRequest(APPLICATION_NAME, "GetAPIKey", new Dictionary<string, string> { { nameof(keyName), keyName } }, Request);
                var result = Processor.TryServiceRequest(request, out WDSServerResponse response);
                returnValue = (result != (int)HttpStatusCode.OK || response.ReturnValue == null) ? null : JsonConvert.DeserializeObject<List<ApiKeyModel>>(response.ReturnValue).FirstOrDefault();
            }

            return new ObjectResult(returnValue);
        }
        #endregion

        #region Applications
        [HttpGet("RefreshApplications")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult RefreshApplications()
        {
            DataServiceSettings.LoadApplicationsFromDB();
            DataServiceSettings.ApiKeys.Load(DataServiceSettings.WDSConnectionString);

            return new ObjectResult(true);
        }

        [HttpGet("GetApplication")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult GetApplication(string appName)
        {
            var app = DataServiceSettings.Applications.Where(o=>o.Key == appName).FirstOrDefault();
            
            if (app.Key == null || app.Value == null)
            {
                return new ObjectResult(null); 
            } else
            {
                return new ObjectResult(app.Value.Model as ApplicationModel);
            }            
        }

        [HttpPost("AddApplication")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult AddApplication([FromBody] ApplicationModel applicationToAdd, bool overrideIfExists)
        {
            var returnValue = false;

            foreach (var rs in applicationToAdd.Recordsets)
                rs.Application = applicationToAdd.Name; 

            if (DataServiceSettings.Applications.ContainsKey(applicationToAdd.Name) && overrideIfExists)
            {
                DataServiceSettings.Applications.Remove(applicationToAdd.Name);
            }

            if (applicationToAdd != null && !applicationToAdd.Name.IsNullOrEmpty() &&
                !DataServiceSettings.Applications.ContainsKey(applicationToAdd.Name) )
            {
                DataServiceSettings.Applications.Add(applicationToAdd.Name, new DataService.Core.Settings.Application(applicationToAdd, true));
                returnValue = true;
            }

            return new ObjectResult(true);
        }

        [HttpPost("AddRecordsetToApplication")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult AddRecordsetToApplication(string appName, [FromBody] RecordsetModel recordsetToAdd)
        {
            var returnValue = false;

            if (!appName.IsNullOrEmpty())
            {
                var rs = DataServiceSettings.Rs(appName, recordsetToAdd?.Name); 
                if (rs == null) // No rs exists
                {
                    returnValue = DataServiceSettings.Applications[appName].AddRecordset(recordsetToAdd, true);
                    returnValue = true; 
                }
            }
            
            return new ObjectResult(returnValue);
        }

        [HttpPost("AddParameterToRecordSet")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult AddParameterToRecordSet(string appName, string recordsetName, ParameterModel parameterToAdd)
        {
            var returnValue = false;
            if (!appName.IsNullOrEmpty() && !recordsetName.IsNullOrEmpty()) 
            {
                var rs = DataServiceSettings.Rs(appName, recordsetName);

                if (rs != null)
                {
                    DataServiceSettings.Applications[appName].Recordsets[recordsetName].AddParameter(parameterToAdd, true);
                    returnValue = true; 
                }
            }

            return new ObjectResult(returnValue);
        }

        [HttpPost("AddUserToApplication")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult AddUserToApplication(int edipi, char ptc, string applicationName, string roleName)
        {
            var returnValue = DataServiceSettings.AddUser(edipi, ptc, applicationName, roleName);

            return new ObjectResult(returnValue);
        }
        #endregion

        #region Connections
        [HttpPost("RefreshConnections")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult RefreshConnections()
        {
            var returnValue = false; 

            if (DataServiceSettings.Connections.ConnectionByName.ContainsKey("WDS"))
            {
                DataServiceSettings.Connections.RefreshDatabaseConnections("WDS", true);
                returnValue = true; 
            }

            return new ObjectResult(returnValue);
        }

        [HttpGet("RestoreConnections")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult RestoreConnections(string dateString)
        {
            var returnValue = false;

            if (DataServiceSettings.RestoreConnections(dateString))
            {
                returnValue = true;
            }

            return new ObjectResult(returnValue);
        }
        #endregion

        #region Service Variables
        [HttpPost("ModifyServiceVariable")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult ModifyServiceVariable(string name, string value, string description, string hostName)
        {
            var returnValue = DataServiceSettings.ModifyServiceVariable(name, value, description, hostName);

            return new ObjectResult(returnValue);
        }

        [HttpPost("GetServiceVariables")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult GetServiceVariables()
        {
            return new ObjectResult(DataServiceSettings.WDSServiceVariables);
        }

        #endregion

        #region Settings Operations


        [HttpPost("BackupSettings")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Write")]
        public IActionResult BackupSettings()
        {
            var returnValue = false;

            if (DataServiceSettings.Backup())
            {
                returnValue = true;
            }

            return new ObjectResult(returnValue);
        }

        [HttpGet("RestoreApp")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult RestoreApp(string dateString, string appName)
        {
            var returnValue = false;

            if (DataServiceSettings.RestoreApp(dateString, appName))
            {
                returnValue = true;
            }

            return new ObjectResult(returnValue);
        }

        

        [HttpGet("RestoreAPIKeys")]
        [Authorize(Policy = Policies.Full, Roles = "WDS.Admin")]
        public IActionResult RestoreAPIKeys(string dateString)
        {
            var returnValue = false;

            if (DataServiceSettings.RestoreAPIKeys(dateString))
            {
                returnValue = true;
            }

            return new ObjectResult(returnValue);
        }
        #endregion
    }
}
