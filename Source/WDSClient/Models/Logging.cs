using static WDSClient.Models.WDSEnums;

namespace WDSClient.Models
{
    public class LogModel
    {
        // Domains will be created in different directories. 
        public string domain;
        public string message;
        public LogLevels LogLevel;
    }
}
