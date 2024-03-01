using Newtonsoft.Json;
using System.Text.Json.Serialization;
using WDS.Core.Extensions;
using static WDSClient.Models.WDSEnums;

namespace WDS.Models
{
    public class WDSRequestContract : IWDSRequestContract
    {
        // This is the unique ID of the request
        private string _id; 
        [JsonProperty("id")]
        public string Id { get { return _id;  } set { _id = value; } }
        private string _applicationName = "";
        /// <summary>
        /// Maps to the plugin (FileName) of the WDS Settings. This property 
        ///     will accept the 'plugin.call' format to set the call at the same 
        ///     time. 
        /// </summary>
        [JsonProperty("Application")]
        public virtual string Application
        {
            get 
            { 
                return (!_applicationName.IsNullOrEmpty() && _applicationName.IndexOf('.') > 2) ? _applicationName.Substring(0, _applicationName.IndexOf('.')) : _applicationName; 
            }
            set
            {
                _applicationName = value;
            }
        }
        private string _call = "";
        /// <summary>
        /// Call within the plugin to be executed
        /// </summary>
        [JsonProperty("call")]
        [JsonPropertyName("call")]
        public virtual string Call
        {
            get
            {
                if (_call.IsNullOrEmpty())
                {
                    if (!_applicationName.IsNullOrEmpty() && _applicationName.IndexOf('.') > 2)
                        _call = _applicationName.Substring(_applicationName.IndexOf('.') + 1);
                    
                }

                return _call;
            }
            set
            { _call = value; }
        }
        /// <summary>
        /// All the paramters that are passed to the plugin.call 
        ///     within the web service. Key = Name and Value = value. 
        /// </summary>
        public Dictionary<string, string> UserParameters { get; set; }
        /// <summary>
        /// Dataformat type for the return value. Can be CSV, Json, XML
        /// </summary>
        [JsonProperty("returnType")]
        [JsonPropertyName("returnType")]
        public DataFormatTypes RequestedDataFormatType { get; set; }
        /// <summary>
        ///     The amount of detail to return in the data. For example if you just want 
        ///     the return data w/o any of the metadata you can select simple. 
        /// </summary>
        [JsonProperty("requestedDataDetailType")]
        [JsonPropertyName("requestedDataDetailType")]
        public DataDetailTypes RequestedDataDetailType { get; set; }
        public WDSRequestContract(string applicationName, string call) : this()
        {
            Application = applicationName;
            Call = call;            
        }
        public WDSRequestContract(string applicationName) : this(applicationName, "")
        {
            
        }
        public WDSRequestContract()
        {
            _id = Guid.NewGuid().ToString();
            RequestedDataDetailType = DataDetailTypes.NotSet;
            UserParameters = new Dictionary<string, string>(); 
        }
    }
}
