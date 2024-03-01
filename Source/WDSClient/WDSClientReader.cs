using WDS.Core.Extensions;
using WDS.Models;
using WDSClient.Models.Users;
using static WDSClient.Models.WDSEnums;

namespace WDSClient
{
    /// <summary>
    /// This class provides a Client Reader that is injected into the WDS Client
    ///     it reads from the appsettings.json file to populate the WDS Client 
    ///     correctly. 
    /// </summary>
    public class WDSClientReader : IWDSClientReader
    {
        /// <summary>
        /// Default plugin that will be called
        /// </summary>
        public string Application { get; protected set; }
        /// <summary>
        /// API Key used to call the DataService
        /// </summary>
        public string APIKey { get; protected set; }
        public UserIdentifier UserId { get; protected set; }
        public List<string> Roles { get; private set;  }
        public DataFormatTypes DefaultDataFormatType { get; set; }
        public DataDetailTypes DataDetailType { get; set; }

        public WDSClientReader()
        {
            Application = SettingsReader.GetAppSetting("AppKeys:WDSClientSettings", "Application");
            APIKey = SettingsReader.GetAppSetting("AppKeys:WDSClientSettings", "API-Key");
            var tempDDFT = SettingsReader.GetAppSetting("AppKeys:WDSClientSettings", "DataFormatType");

            if (!tempDDFT.IsNullOrEmpty())
            {
                DefaultDataFormatType = Enums.ToEnum<DataFormatTypes>(tempDDFT);
            }
            
            var tempDDT = SettingsReader.GetAppSetting("AppKeys:WDSClientSettings", "DataDetailType");
            if (!tempDDT.IsNullOrEmpty())
            {
                DataDetailType = Enums.ToEnum<DataDetailTypes>(tempDDT);
            }
        }
        public WDSClientReader(string userId): this()
        {
            UserId = new UserIdentifier(userId);
        }

        public WDSClientReader(string applicationName, string aPIKey, string userId, DataFormatTypes defaultDataFormatType) 
        {
            Application = applicationName;
            APIKey = aPIKey;
            UserId = new UserIdentifier(userId); 
            DefaultDataFormatType = defaultDataFormatType;
        }   
    }
}
