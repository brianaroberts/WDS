using static WDSClient.Models.WDSEnums;

namespace WDS.Models
{
    public interface IWDSResponseContract
    {
        public DataFormatTypes ResponseDataFormatType { get; set; }
        public WDSRequestContract Request { get; set; }
        public string ExecutionDate { get; }
        public string ExecutionDuration { get; }
        public string SQLExecutionDuration { get; }
        public bool ContainsError { get; }
        public string ErrorMessage { get; set; }
        public string ReturnValue { get; set; }
    }
}
