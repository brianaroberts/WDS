using DataService.Core.Data;

namespace DataService.Models
{
    public class ParameterExampleValueModel
    {
        [DataNames("set_name")]
        public string SetName {  get; set; }
        [DataNames("name")]
        public string Name { get; set; }
        [DataNames("value")]
        public string Value { get; set; }
    }
}
