using System.Collections.Generic;

namespace DataService.Models.Settings
{
    public interface IRecordSet : IWDSServerRequest
    {
        public string ConnectionName { get; set; }
        public List<ParameterModel> Parameters { get; set; }
    }
}
