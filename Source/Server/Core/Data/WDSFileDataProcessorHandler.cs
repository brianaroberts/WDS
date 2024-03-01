using DataService.Models.Settings;
using DataService.Models;
using System.Collections.Generic;
using DataService.Core.Data;
using static WDSClient.Models.WDSEnums;

namespace WDS.Data
{
    public class WDSFileDataProcessorHandler : IWDSCallProcessor
    {
        public List<CallTypes> SupportedProcessorType { get => new List<CallTypes>() { CallTypes.GetDataFromFile }; }

        public WDSServerResponse MakeCall(WDSServerRequest request)
        {
            return FileOperations.GetDataFromFile(request);
        }
    }
}
