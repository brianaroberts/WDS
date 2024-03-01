using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Diagnostics;
using WDS.Core.Extensions;
using static WDSClient.Models.WDSEnums;

namespace WDS.Models
{
    public class WDSResponseContract : IWDSResponseContract
    {
        public DataFormatTypes ResponseDataFormatType { get; set; }
        public DataDetailTypes RequestedDataDetailType { get; set; }
        [JsonProperty("executionDate")]
        public string ExecutionDate { get; set; }
        [JsonProperty("executionDuration")]
        public string ExecutionDuration { get; set; }
        [JsonProperty("sqlExecutionDuration")]
        public string SQLExecutionDuration { get; set; }
        private WDSRequestContract _request; 
        [JsonProperty("request")]
        public WDSRequestContract Request {
            get 
            { 
                if (_request == null)
                    _request = new WDSRequestContract();

                return _request;
            }
            set 
            {
                _request = value;
            } 
        }
        [JsonProperty("returnValue")]
        public string ReturnValue { get; set; }
        public bool ContainsError { get { return !string.IsNullOrEmpty(ErrorMessage); } }
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        public WDSResponseContract(DataDetailTypes dataDetailType)
        {
            RequestedDataDetailType = dataDetailType;
        }

        public WDSResponseContract(string error)
        {
            ErrorMessage = error;
        }

        public WDSResponseContract()
        {
            
        }

        public object GetHttpResponse()
        {
            switch (RequestedDataDetailType)
            {
                case DataDetailTypes.Simple:
                    return ReturnValue;
                case DataDetailTypes.Default:
                default:
                                     
                    return this;
            }
        }
        public DataTable GetData()
        {
            return GetMappedData<DataTable>(); 
        }
        public T GetMappedData<T>() where T : new()
        {
            var _returnDataTable = new T();

            if (ReturnValue != null)
            {
                switch (ResponseDataFormatType)
                {
                    case DataFormatTypes.NotSet:
                    case DataFormatTypes.Json:
                        _returnDataTable = JsonConvert.DeserializeObject<T>(ReturnValue);
                        break;
                    case DataFormatTypes.CSV:
                        // TODO: This is not very effecient
                        var dt = new DataTable();
                        dt.LoadCsv(ReturnValue);
                        var json = dt.ToJson();
                        _returnDataTable = JsonConvert.DeserializeObject<T>(json);
                        break;
                    case DataFormatTypes.Email:
                        ErrorMessage += $"{ResponseDataFormatType} is not currently supported as a datatable type.";
                        break;
                    case DataFormatTypes.Xml:
                        DataTable xmlDt = ReturnValue.FromXml();
                        var xmlJson = xmlDt.ToJson();
                        _returnDataTable = JsonConvert.DeserializeObject<T>(xmlJson); 
                        break;
                    default:
                        ErrorMessage += $"{ResponseDataFormatType} is not currently supported by the WDS Client, please contact support for this API.";
                        break;
                }
            }
            else
            {
                ErrorMessage += $"ReturnValue is null, mapped data can not be processed";
            }

            return _returnDataTable;
        }
    }
}


