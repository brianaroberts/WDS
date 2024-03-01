using WDS.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using DataService.Models;
using System;
using DataService.Core;
using System.Collections.Generic;
using System.Linq;
using DataService.Models.User;
using DataService.Core.Settings;
using System.Xml;
using System.Net;
using DataService.Models.Settings;

namespace WDS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class AuthorizationController : WDSControllerBase
    {
        private const string APPLICATION_NAME = "Authorization"; 
        public AuthorizationController(): base(APPLICATION_NAME)
        {

        }

        [HttpPost("GetSessionID")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public ActionResult<string> GetSessionIDForApplication()
        {
            return new ObjectResult(GetSessionID());
        }

        [HttpPost("GetSessionIDApplication")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public ActionResult<string> GetSessionIDApplication(string sessionId)
        {
            if (WDSCache.TryGetSession(sessionId, out var returnSession))
                return new ObjectResult(returnSession);
            else
                return new ObjectResult(null);
        }

        [HttpPost("GetUserRoles")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public ActionResult<List<ApplicationUserRoleDTO>> GetUserRoles(string userId, string applicationName)
        {
            var dodObj = new WDSClient.Models.Users.UserIdentifier(userId);

            return new ObjectResult(GetUserRoles(dodObj, applicationName));
        }

        [HttpPost("GetUserClaims")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public ActionResult<List<ClaimsDTO>> GetUserClaims(string userId, string applicationName)
        {
            var dodObj = new WDSClient.Models.Users.UserIdentifier(userId);
            
            return new ObjectResult(GetUserClaims(dodObj, applicationName));
        }

        [HttpPost("GetUser")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public ActionResult<UserDTO> GetUser(string userId)
        {
            var dodObj = new WDSClient.Models.Users.UserIdentifier(userId);

            return new ObjectResult(GetUser(dodObj));
        }

        [HttpPost("GetApplicationUser")]
        [Authorize(Policy = Policies.Read, Roles = "WDS.Read")]
        public ActionResult<ApplicationUserDTO> GetApplicationUser(string userId, string applicationName)
        {
            var dodObj = new WDSClient.Models.Users.UserIdentifier(userId);
            ApplicationUserDTO returnValue = new ApplicationUserDTO(GetUser(dodObj));
            if (returnValue != null)
            {
                var parameters = new Dictionary<string, string>() { { "edipi", dodObj.EDIPI.ToString() }, { nameof(applicationName), applicationName } };

                returnValue.Roles = GetUserRoles(dodObj, applicationName);
                returnValue.Claims = GetUserClaims(dodObj, applicationName);

                var request = new WDSServerRequest(APPLICATION_NAME, "LogUserApplicationAccess", new Dictionary<string, string>
                        {
                            { "edipi", dodObj.EDIPI.ToString() },
                            { "ptc", dodObj.PTC.ToString() },
                            { "app_name", applicationName }
                        }
                    , Request);
                Processor.ProcessRequestAsync(request);

                returnValue.SessionKey = GetSessionID(); // Request is part of object & SessionID is cached. 
            }
            return new ObjectResult(returnValue);            
        }

        #region NonAPI Functions
        public UserDTO GetUser(WDSClient.Models.Users.UserIdentifier userObj)
        {
            if (WDSCache.TryGetUsers(userObj.EDIPI.ToString(), out var user))
                return user;

            var returnValue = GetUserFromPrimaryDB(userObj);

            if (returnValue == null)
                returnValue = GetUserFromSecondaryDB(userObj);

            return returnValue;
        }

        private UserDTO GetUserFromPrimaryDB(WDSClient.Models.Users.UserIdentifier userObj)
        {
            UserDTO returnValue = null;
            var parameters = new Dictionary<string, string>() { { "edipi", userObj.EDIPI.ToString() }, { "ptc", userObj.PTC.ToString() } };
            // First we will try Pandora
            var request = new WDSServerRequest(APPLICATION_NAME, "GetUser", parameters, Request);
            var statusCode = Processor.TryServiceRequest(request, out WDSServerResponse response);

            if (statusCode != (int)HttpStatusCode.OK) return returnValue; 

            var users = response.GetMappedData<List<UserDTO>>();

            if (users != null && users.Count > 0)
            {
                returnValue = users.First();
                WDSCache.AddUser(userObj.EDIPI.ToString(), returnValue);
            }

            return returnValue;
        }

        private UserDTO GetUserFromSecondaryDB(WDSClient.Models.Users.UserIdentifier userObj)
        {
            UserDTO returnValue = null;
            var parameters = new Dictionary<string, string>() { { "edipi", $"{userObj.EDIPI}" }, { "ptc", $"{userObj.PTC}" } };
            // First we will try Pandora
            var request = new WDSServerRequest(APPLICATION_NAME, "GetUser2", parameters, Request); ;
            var statusCode = Processor.TryServiceRequest(request, out WDSServerResponse response);

            if (statusCode != (int)HttpStatusCode.OK) return returnValue;

            var users = response.GetMappedData<List<UserDTO>>();

            if (users != null && users.Count > 0)
            {
                returnValue = users.First();
                WDSCache.AddUser(userObj.EDIPI.ToString(), returnValue);
            }

            return returnValue;
        }

        private string GetSessionID()
        {
            var apiKey = AuthenticationCommon.GetAPIKey(Request);
            var newSessionKey = Guid.NewGuid().ToString();

            if (DataServiceSettings.ApiKeys.DoesKeyExist(apiKey))
            {
                WDSCache.AddSession(newSessionKey, apiKey);
            }
            return newSessionKey;
        }

        private List<ClaimsDTO> GetUserClaims(WDSClient.Models.Users.UserIdentifier userobj, string applicationName)
        {
            var request = new WDSServerRequest(APPLICATION_NAME, "GetClaims", new Dictionary<string, string> {
                    { "edipi", userobj.EDIPI.ToString() },
                    { nameof(applicationName), applicationName }
                }, Request);

            var statusCode = Processor.TryServiceRequest(request, out WDSServerResponse response);

            if (statusCode != (int)HttpStatusCode.OK || response == null) return null; 

            var returnValue = response.GetMappedData<List<DataService.Models.User.ClaimsDTO>>();

            return returnValue;
        }

        private List<ApplicationUserRoleDTO> GetUserRoles(WDSClient.Models.Users.UserIdentifier userobj, string applicationName)
        {
            var request = new WDSServerRequest(APPLICATION_NAME, "GetRoles", new Dictionary<string, string> {
                { "edipi", userobj.EDIPI.ToString() },
                { nameof(applicationName), applicationName }
                }, Request);

            var statusCode = Processor.TryServiceRequest(request, out WDSServerResponse response);

            if (statusCode != (int)HttpStatusCode.OK || response == null) return null;

            var returnValue = response.GetMappedData<List<DataService.Models.User.ApplicationUserRoleDTO>>();

            return returnValue;
        }
        #endregion
    }
}
