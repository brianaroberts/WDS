using static WDSClient.Models.WDSEnums;

namespace WDS.Models
{
    public interface IWDSRequestContract
    {
        public string Application { get; }
        public string Call { get; }
        public DataFormatTypes RequestedDataFormatType { get; }
        public DataDetailTypes RequestedDataDetailType { get; }
        public Dictionary<string,string> UserParameters { get; }
    }
}
