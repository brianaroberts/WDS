using DataService.Models.Settings;
using DataService.Models;
using System.Collections.Generic;
using static WDSClient.Models.WDSEnums;

namespace WDS.Data
{
    public interface IWDSCallProcessor
    {
        public List<CallTypes> SupportedProcessorType { get; }
        WDSServerResponse MakeCall(WDSServerRequest request);
    }
}
