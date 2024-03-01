using WDS.Models;
using static WDSClient.Models.WDSEnums;

namespace DataService.Models.Settings
{
    public interface IWDSServerRequest : IWDSRequestContract
    {  
        public CallTypes CallType { get; }
        public string Statement { get; }
        
    }
}
