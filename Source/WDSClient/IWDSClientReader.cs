using WDS.Models;
using WDSClient.Models.Users;
using static WDSClient.Models.WDSEnums;

namespace WDSClient
{
    public interface IWDSClientReader
    {
        public string Application { get; }
        public string APIKey { get; }
        public UserIdentifier UserId { get; }
        public DataFormatTypes DefaultDataFormatType { get; }
    }
}
