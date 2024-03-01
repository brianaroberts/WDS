using Newtonsoft.Json;
using System.Data;
using static WDS.Settings.ConnectionsModel;
using static DataService.Core.Settings.DataServiceSettings;
using System.Text.Json.Serialization;
using WDS.Core.Extensions;
using DataService.Core.Settings;
using DataService.Core.Data;
using static WDSClient.Models.WDSEnums;

namespace WDS.Settings
{
    public class ConnectionModel
    {
        
        public ConnectionTypes ConnectionType { get; set; }
        public SourcedFromTypes SourcedFrom;

        [JsonProperty("ConnectionID")]
        [JsonPropertyName("ConnectionID")]
        public int ConnectionID { get; set; }
        private string _hostName = string.Empty;
        [JsonProperty("HostName")]
        [JsonPropertyName("HostName")]
        public string HostName { 
            get
            {
                if (_hostName.IsNullOrEmpty())
                    _hostName = DataServiceSettings.Connections.HostName; 
                return _hostName;
            } 
            set
            {
                _hostName = value;
            }
        }
        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
        [DataNames("name")]
        public string Name { get; set; }
        [JsonProperty("Production")]
        [JsonPropertyName("Production")]
        [DataNames("production")]
        public string Production { get; set; }
        [JsonProperty("Staging")]
        [JsonPropertyName("Staging")]
        [DataNames("staging")]
        public string Staging { get; set; }
        [JsonProperty("Development")]
        [JsonPropertyName("Development")]
        [DataNames("development")]
        public string Development { get; set; }
        [JsonProperty("Type")]
        [JsonPropertyName("Type")]
        [DataNames("connection_type")]
        public string Type { get; set; }
        [JsonProperty("InsertIntoDB")]
        [JsonPropertyName("InsertIntoDB")]
        public bool InsertIntoDB { get; set; }
        public ConnectionModel()
        {

        }

        public ConnectionModel(DataRow dbRow)
        {
            if (dbRow != null)
            {
                Name = dbRow["name"].ToString();
                Type = dbRow["connection_type"].ToString();
                Production = dbRow["production"].ToString();
                Staging = dbRow["staging"].ToString();
                Development = dbRow["development"].ToString();
            }           
        }

        public string GetConnection()
        {
            var returnValue = string.Empty;

            switch (ConnectionsModel.ConnectionEnvironment)
            {
                case ConnectionEnvironmentTypes.Production:
                    returnValue = Production;
                    break;  
                case ConnectionEnvironmentTypes.Staging:
                    returnValue = Staging;
                    break;
                case ConnectionEnvironmentTypes.DevelopmentFullAccess:
                case ConnectionEnvironmentTypes.Development:
                    returnValue = Development;
                    break;
            }

            return returnValue;
        }
    }
}
