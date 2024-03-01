using DataService.Models.Settings;
using DataService.Models;
using System.Collections.Generic;
using DataService.Core.Data;
using static WDSClient.Models.WDSEnums;

namespace WDS.Data
{
    public class WDSDailyFileUpdateProcessorHandler : IWDSCallProcessor
    {
        public List<CallTypes> SupportedProcessorType { get => new List<CallTypes>() { CallTypes.DailyFileUpdate }; }

        public WDSServerResponse MakeCall(WDSServerRequest request)
        {
            return FileOperations.UpdateFile(request, isDatePartitioned: true);
        }
    }
}
