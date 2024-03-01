using DataService.Core.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using WDS.Models;
using static WDSClient.Models.WDSEnums;

namespace DataService.Models.Settings
{
    public class RecordsetModel
    {
        [JsonProperty("Application")]
        [JsonPropertyName("Application")]
        [DataNames("application")]
        public string Application { get; set; }
        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
        [DataNames("name", "recordset")]
        public string Name { get; set; }
        [JsonProperty("Substitute")]
        [JsonPropertyName("Substitute")]
        [DataNames("substitute")]
        public string Substitute { get; set; }
        [JsonProperty("ConnectionName")]
        [JsonPropertyName("ConnectionName")]
        [DataNames("connection_name")]
        public string ConnectionName { get; set; }
        private CallTypes _callType = CallTypes.NotSet;
        [JsonProperty("CallType")]
        [JsonPropertyName("CallType")]
        [DataNames("callType_id")]
        public CallTypes CallType
        { 
            get
            {
                if (_callType.Equals(CallTypes.NotSet))
                    _callType = CallTypes.SQL;
                return _callType;
            } 
            set 
            {
                _callType = value;
            } 
        }
        private DataFormatTypes _returnType= DataFormatTypes.NotSet;
        [JsonProperty("ReturnType")]
        [JsonPropertyName("ReturnType")]
        [DataNames("returnType_id")]
        public DataFormatTypes ReturnType
        {
            get
            {
                if (_returnType.Equals(DataFormatTypes.NotSet))
                    _returnType = DataFormatTypes.Json;
                return _returnType;
            }
            set
            {
                _returnType = value;
            }
        }
        private DataDetailTypes _dataDetailsType;
        [JsonProperty("DataDetailType")]
        [JsonPropertyName("DataDetailType")]
        [DataNames("returnType_details_id")]
        public DataDetailTypes DataDetailType 
        {
            get
            {
                if (_dataDetailsType.Equals(DataDetailTypes.NotSet))
                    _dataDetailsType = DataDetailTypes.Default; 
                return _dataDetailsType;
            }
            set
            {
                _dataDetailsType = value;
            }
        }
        [JsonProperty("Statement")]
        [JsonPropertyName("Statement")]
        [DataNames("statement")]
        public string Statement { get; set; }
        [JsonProperty("Cache")]
        [JsonPropertyName("Cache")]
        [DataNames("cache")]
        public bool IsCached { get; set; }
        [JsonProperty("CacheTimeOut")]
        [JsonPropertyName("CacheTimeOut")]
        [DataNames("cache_timeout")]
        public int CacheTimeOut { get; set; }
        [JsonProperty("EndPointRequired")]
        [DefaultValue(true)]
        [JsonPropertyName("EndPointRequired")]
        [DataNames("endpoint_required")]
        public bool EndPointRequired { get; set; }
        [JsonProperty("LogCallInDB")]
        [DefaultValue(false)]
        [JsonPropertyName("LogCallInDB")]
        [DataNames("log_call")]
        public bool LogCallInDB { get; set; }        
        [JsonProperty("Parameters")]
        [JsonPropertyName("Parameters")]
        public List<ParameterModel> Parameters { get; set; }
        [JsonProperty("Role")]
        [JsonPropertyName("Role")]
        [DataNames("role")]
        public string Role { get; set; }

        public RecordsetModel() 
        {

        }

        public static implicit operator RecordsetModel(string v)
        {
            throw new NotImplementedException();
        }
    }
}
