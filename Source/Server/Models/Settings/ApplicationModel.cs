using DataService.Core.Data;
using DataService.Models.Settings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using WDS.Models;
using static DataService.Core.Settings.DataServiceSettings;
using static System.Net.Mime.MediaTypeNames;
using static WDSClient.Models.WDSEnums;

namespace WDS.Settings
{
    public class ApplicationModel
    {
        public SourcedFromTypes SourcedFrom;

        [JsonProperty("Name")]
        [JsonPropertyName("Name")]
        [DataNames("name")]
        public string Name { get; set; }
        [JsonProperty("DataOwner")]
        [JsonPropertyName("DataOwner")]
        [DataNames("owner")]
        public string DataOwner { get; set; } = string.Empty;
        [JsonProperty("Description")]
        [JsonPropertyName("Description")]
        [DataNames("description")]
        public string Description { get; set; } = string.Empty;
        [JsonProperty("Recordsets")]
        [JsonPropertyName("Recordsets")]
        public List<RecordsetModel> Recordsets { get; set; } = new List<RecordsetModel>();         
    }
}
