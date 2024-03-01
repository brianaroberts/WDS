using WDS.Core.Extensions;
using System.Collections.Generic;
using System.Linq;
using WDS.Models;
using WDSClient.Models.Users;
using Microsoft.AspNetCore.Http;
using WDS.Authentication;
using DataService.Core.Settings;
using System;
using static WDSClient.Models.WDSEnums;
using System.ComponentModel.DataAnnotations;

namespace DataService.Models.Settings
{
    public class WDSServerRequest : WDSRequestContract, IRequestParameters
    {
        private const DataFormatTypes _DEFAULT_DATA_FORMAT_TYPE = DataFormatTypes.CSV;
        private DataFormatTypes _responseDataFormatType;
        private Recordset _recordSet;
        private string _application = string.Empty; 
        private string _callId = string.Empty; 
        private UserIdentifier _user;
        private string _errorMessage = string.Empty;

        public ApiKeyModel ApiKey = null;

        public bool IsCallLoggedInDB { get; set; } = false;
        public override string Application
        {
            get
            {
                return _application;
            }
            set
            {
                if (DataServiceSettings.Applications.ContainsKey(value))
                    _application = value;
                else
                {
                    _errorMessage += $"The application:{value} is not a valid one within the DataService. ";
                    _application = value;
                }
            }
        }
        public override string Call
        {
            get { return _callId; }
            set 
            {
                if (DataServiceSettings.Applications.ContainsKey(Application) && DataServiceSettings.Applications[Application].Recordsets.ContainsKey(value))
                {
                    _recordSet = DataServiceSettings.Applications[Application].Recordsets[value];
                    _callId = value;
                }
                else
                {
                    _errorMessage += $"{Application}.{value} is not a valid application call for the supplied endpoint. Did you mean to call an endpoint directly?";
                    _callId = value;
                }
            }
        }
        public string Host { get; set; }        
        public CallTypes CallType
        {
            get
            {
                if (_recordSet == null || _recordSet.Model == null)
                    return CallTypes.NotSet; 
                else
                    return _recordSet.Model.CallType;
            }
            set
            {
                if (_recordSet == null || _recordSet.Model == null)
                {
                    SetRecordset();
                }
                _recordSet.Model.CallType = value;
            }   
        }
        public DataFormatTypes ResponseDataFormatType
        {
            get
            {
                // Currently Designed Precedent: User => Application Configuration => Default
                if (_responseDataFormatType == DataFormatTypes.NotSet)
                {
                    if (RequestedDataFormatType != DataFormatTypes.NotSet)
                        _responseDataFormatType = RequestedDataFormatType;
                    else if (_recordSet?.Model.ReturnType != null)
                        _responseDataFormatType = _recordSet.Model.ReturnType;
                    else
                        _responseDataFormatType = _DEFAULT_DATA_FORMAT_TYPE;
                }
                return _responseDataFormatType;                 
            }
            set 
            {
                if (_recordSet == null)
                    _recordSet = new Recordset(new RecordsetModel() { ReturnType = value});
            }
        }
        public string Statement
        {
            get
            {
                if (_recordSet == null)
                    return "";
                else
                    return UserParameters.ReplaceKeysWithValuesInString(_recordSet.Model.Statement);
            }
            set
            {
                if (_recordSet == null)
                    _recordSet = new Recordset(new RecordsetModel() { Statement = value });
                else
                    _recordSet.Model.Statement = value; 
            }
        }
        
        public UserIdentifier User
        {
            get { return _user; }
        }
        public Dictionary<string, string> CallParameters
        {
            get
            {
                if (_recordSet == null || _recordSet.Parameters == null || UserParameters == null)
                    return new Dictionary<string, string>();
                else
                    return _recordSet.Parameters.
                        ToDictionary(p => p.Value.Model.RecordsetParameter, p => (UserParameters.ContainsKey(p.Key) && !UserParameters[p.Key].IsNullOrEmpty()) ? MacroReplaceWithUserParameters(UserParameters[p.Key], p.Value.Model.ReplaceWith) : MacroReplaceWithUserParameters(p.Value.Model.DefaultValue, MacroReplaceWithUserParameters("", p.Value.Model.ReplaceWith)));
            }
            set
            {
                return;
            }
        }
        public bool NeedsEndpoint
        {
            get { return (_recordSet != null && _recordSet.Model != null) ? _recordSet.Model.EndPointRequired : true; }
        }
        public string ErrorMessage { 
            get { return _errorMessage; }
        }
        public bool ContainsError { get
            {
                return !_errorMessage.IsNullOrEmpty();
            }  
        }

        #region Constructors
        public WDSServerRequest()
        {

        }
        
        public WDSServerRequest(string application, string call, Dictionary<string, string> parameterValues, HttpRequest req) 
        {
            var userId = ""; 
            Application = application;
            Call = call;
            UserParameters = parameterValues;
            Host = req.Host.ToString();

            if (req != null)
            {
                req.Headers.TryGetValue(HttpHeaderNameConstants.ApiKey, out var returnValue);
                ApiKey = DataServiceSettings.ApiKeys.RetrieveKey(returnValue); 

                if (req.Headers.TryGetValue(HttpHeaderNameConstants.CallerIdentity, out var callerIdHeaderValues))
                    userId = callerIdHeaderValues.FirstOrDefault("");
            }

            SetRecordset();
        }

        public WDSServerRequest(WDSServerRequest req, HttpRequest httpReq) : this(req.Application, req.Call, req)
        {
            
        }

        public WDSServerRequest(string application, string call, WDSServerRequest req) : this(application, call, null, req.Host, req._user)
        {
            RequestedDataDetailType = req.RequestedDataDetailType;
            RequestedDataFormatType = req.RequestedDataFormatType;
        }

        public WDSServerRequest(string application, string call, Dictionary<string, string> parameterValues, string host, UserIdentifier user)
        {
            Application = application;
            Call = call;
            UserParameters = parameterValues;
            Host = host;
            _user = user;
        }

        #endregion Constructors

        #region Private Methods
        private string MacroReplaceWithUserParameters(string original, string toReplace)
        {
            if (toReplace.IsNullOrEmpty())
            {
                return original;
            }
            else
            {
                return toReplace.ReplaceWith(UserParameters).ReplaceWith(DataServiceSettings.SystemEnvVariables).ReplaceWith(DataServiceSettings.WDSServiceVariables); 
            }
        }
        #endregion

        public void SetRecordset()
        {
            if (DataServiceSettings.Applications.ContainsKey(Application) && DataServiceSettings.Applications[Application].Recordsets.ContainsKey(Call))
                _recordSet = DataServiceSettings.Applications[Application].Recordsets[Call];
            else
                _recordSet = new Recordset();
        }
        public bool IsRequestValid()
        {
            return _recordSet == null ? false : _recordSet.AreParametersValid(UserParameters);
        }
        public bool LogUserCallToDB()
        {
            return _recordSet == null ? false : ((!IsCallLoggedInDB) && (_recordSet.Model.LogCallInDB || DataServiceSettings.LogLevel == LogLevels.Information));
        }
        public string GetConnectionName()
        {            
            if (_recordSet == null)
                return string.Empty;
            else
                return _recordSet.Model.ConnectionName;
        }
        public override int GetHashCode()
        {
            int result = Application.GetHashCode();
            result = (result * 397) ^ Id.GetHashCode();
            foreach (var cp in CallParameters)
                result = (((result * 397) ^ cp.Key.GetHashCode())* 397) ^ cp.Value.GetHashCode(); 

            return result;
        }
        public void SetUserIdentity(string id)
        {
            _user = new UserIdentifier(id); 
        }
        public string ToString()
        {
            return $"{Application}.{Call};Statement={Statement};Parameters={{ {CallParameters.ToString(";")} }}";
        }
        public void SetDataDetailTypeWithApplication()
        {
            if (RequestedDataDetailType == DataDetailTypes.NotSet)
            {
                var rs = DataServiceSettings.Rs(Application, Call);
                if (rs == null || rs.Model.DataDetailType == DataDetailTypes.NotSet)
                    RequestedDataDetailType = DataDetailTypes.Default;
                else
                    RequestedDataDetailType = rs.Model.DataDetailType; 
            }
        }
        internal void Sanitize()
        {
            // Clear our all sensitive information
            Statement = "See settings";
        }
        public void SetToHasBeenLogged()
        {
            IsCallLoggedInDB = true;
        }

        public WDSServerRequest GetSubstitute()
        {
            if (_recordSet.Model.Substitute.IsNullOrEmpty()) return null; 

            var sep = _recordSet.Model.Substitute.IndexOf("."); 
            var application = (sep > 0) ? _recordSet.Model.Substitute.Substring(0, sep) : Application;
            var rs = (sep < 0) ? _recordSet.Model.Substitute : _recordSet.Model.Substitute.Substring(sep + 1); 
            
            return new WDSServerRequest(application, rs, this);
        }
    }
}
