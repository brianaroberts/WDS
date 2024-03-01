using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static DataService.Core.Settings.DataServiceSettings;

namespace WDS.Settings
{
    public class ConnectionsModel
    {
        public enum ConnectionEnvironmentTypes { Production, Staging, Development, DevelopmentFullAccess };
        public static ConnectionEnvironmentTypes ConnectionEnvironment = ConnectionEnvironmentTypes.Production;

        [JsonPropertyName("Connections")]
        [JsonProperty("Connections")]
        public List<ConnectionModel> Connections = new List<ConnectionModel>(); 
    }
}
