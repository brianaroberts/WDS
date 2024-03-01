using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using WDS.Authentication;
using WDS.Core.Extensions;
using WDS.Models;
using WDSClient.Models;
using WDSClient.Models.Users;
using static WDSClient.Models.WDSEnums;

namespace WDSClient
{
    public class Client
    {
        #region Properties
        private IWDSClientReader _reader;

        private HttpClient _httpClient = new HttpClient();
        private List<WDSRequestContract> _calls = new List<WDSRequestContract>();
        private List<WDSRequestContract> _logs = new List<WDSRequestContract>();
        private List<WDSClientResult> _responses = new List<WDSClientResult>();
        private UserIdentifier _userId = null; 
        private ApplicationUser _user;

        public Dictionary<string, WDSClientResult> Responses
        {
            get
            {
                int i = 1;
                return _responses.ToDictionary(o => o.Request == null ? $"Unknown-{i++}" : o.Request.Id, o => o);
            }
        }
        public ApplicationUser User
        {
            get
            {
                if (_user == null && _userId != null)
                {
                    _user = ApplicationUser.GetApplicationUser(_userId, _reader.Application, this);
                }
                return _user;
            }
        }
        public string WDSServer { get; protected set; }
        #endregion

        #region Constructors
        public Client(IWDSClientReader reader)
        {
            _reader = reader;
            _userId = _reader.UserId;
            
            if (_userId.EDIPI < 1)
                _userId = new UserIdentifier(ADUser.GetEDIPIUsingAD(_reader.UserId.ADUserName, this)); 
            
            WDSServer = SettingsReader.WDSServer;
            _httpClient.BaseAddress = new Uri(SettingsReader.WDSServer);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add(HttpHeaderNameConstants.ApiKey, $"{_reader.APIKey}");
            _httpClient.DefaultRequestHeaders.Add(HttpHeaderNameConstants.CallerIdentity, $"{_userId}");
        }
       
        #endregion
        public Client AddCall(string call, Dictionary<string, string> parameters, DataFormatTypes returnType = DataFormatTypes.NotSet)
        {
            var _call = call.IndexOf(".") > 0 ? call : $"{_reader.Application}.{call}";
            _calls.Add(new WDSRequestContract(_call) { UserParameters = parameters, RequestedDataFormatType = returnType, RequestedDataDetailType = DataDetailTypes.Default });
            return this;
        }

        public Client AddCall(string call, string parameters = "", DataFormatTypes returnType = DataFormatTypes.NotSet)
        {
            return AddCall(call, parameters.ToDictionary(), returnType); 
        }
        public Client WriteLog(string area, string message, LogLevels logLevels, bool sendNow = true)
        {
            var parameters = new Dictionary<string, string>()
            {
                { "host", Environment.MachineName },
                { "domain", _reader.Application },
                { "area", area },
                { "message", message },
                { "logLevel", ((int)logLevels).ToString() }
            }; 
            _logs.Add(new WDSRequestContract("WDSLogging.ApplicationLog") { UserParameters = parameters, RequestedDataFormatType = DataFormatTypes.NotSet });
            if (sendNow)
            {
                MakeCallsAsync("Log/GetMulti", _logs);
                _logs.Clear(); 
            }
            return this; 
        }
        public async Task SendAsync()
        {
            _responses = await MakeCallsAsync("WDS/GetMulti", _calls);
            _calls.Clear();
        }
        public void Send()
        {
            SendAsync();
            _calls.Clear();
        }
        public async Task<List<WDSClientResult>> MakeCallsAsync(string ws, List<WDSRequestContract> calls)
        {
            var returnValue = new List<WDSClientResult>();
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(calls), Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync($"{SettingsReader.WDSServer}/{ws}", content).Result;

                if (response == null)
                {
                    throw new NullReferenceException($"Response from the server {WDSServer} came back null.");
                }
                else if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    
                    returnValue = JsonConvert.DeserializeObject<List<WDSClientResult>>(result);
                }
                else
                {
                    throw new Exception($"Call was not successful: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                foreach (var call in calls)
                    returnValue.Add(new WDSClientResult(e.Message));                
            }
            
            return returnValue;
        }

        public string GetRequestBody()
        {
            return JsonConvert.SerializeObject(_calls); 
        }

        public WDSClientResult MakeCall(WDSRequestContract call)
        {
            return MakeCall(call.Application, call); 
        }
        public WDSClientResult MakeCall(string ws, WDSRequestContract call)
        {
            WDSClientResult returnValue;
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(call), Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync($"{SettingsReader.WDSServer}/{ws}", content).Result;
                
                if (response == null)
                {
                    throw new NullReferenceException($"Reponse from the server {WDSServer} came back null.");
                }
                else if (response.IsSuccessStatusCode)
                {
                    returnValue = JsonConvert.DeserializeObject<WDSClientResult>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    throw new Exception($"Call was not successful: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                returnValue = new WDSClientResult(e.Message);
            }

            return returnValue;
        }
        public T MakeCall<T>(string ws, WDSRequestContract call)
        {
            T returnValue;
          
                var content = new StringContent(JsonConvert.SerializeObject(call), Encoding.UTF8, "application/json");
                var response = _httpClient.PostAsync($"{SettingsReader.WDSServer}/{ws}", content).Result;

                if (response == null)
                {
                    throw new NullReferenceException($"Reponse from the server {WDSServer} came back null.");
                }
                else if (response.IsSuccessStatusCode)
                {
                    returnValue = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception($"You do not have access to the endpoint '{ws}'. Get with the WDS Administrator to get access/apikey.");
                } else
                {
                    throw new Exception($"Call was not successful: {response.StatusCode}");
                }


            if (returnValue == null)
                throw new Exception("Unable to make call, object returned null.");

            return returnValue;
        }
    }
}