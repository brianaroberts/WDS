using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json.Serialization;
using DataService.Core.Data;

namespace DataService.Models.Settings
{
    public class ParameterModel
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        [DataNames("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("recordsetParameter")]
        [JsonPropertyName("recordsetParameter")]
        [DataNames("call_parameter_name")]
        public string RecordsetParameter { get; set; } = string.Empty;
        [DefaultValue(true)]
        [JsonProperty("required", DefaultValueHandling = DefaultValueHandling.Populate)]
        [JsonPropertyName("required")]
        [DataNames("required")]
        public bool Required { get; set; }
        [JsonProperty("regExMask")]
        [JsonPropertyName("regExMask")]
        [DataNames("reg_ex_mask")]
        public string RegularExpression { get; set; } = string.Empty;
        [JsonProperty("defaultValue")]
        [JsonPropertyName("defaultValue")]
        [DataNames("default_value")]
        public string DefaultValue { get; set; } = string.Empty;
        // TODO: replaceWith shoud be conditional, need to figure that out.
        [JsonProperty("replaceWith")]
        [JsonPropertyName("replaceWith")]
        [DataNames("replace_with")]
        public string ReplaceWith { get; set; } = string.Empty;
    }
}
