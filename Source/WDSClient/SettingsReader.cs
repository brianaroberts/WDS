using Microsoft.Extensions.Configuration;
using System.IO;

namespace WDSClient
{
    public class SettingsReader
    {
        private static SettingsReader _appSettings;

        public string appSettingValue { get; set; }

        public static string AppSetting(string section, string key)
        {
            _appSettings = GetCurrentSettings(section, key);
            return _appSettings.appSettingValue;
        }

        public SettingsReader(IConfiguration config, string Key)
        {
            this.appSettingValue = config.GetValue<string>(Key);
        }

        public static SettingsReader GetCurrentSettings(string section, string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();

            var settings = new SettingsReader(configuration.GetSection(section), key);

            return settings;
        }

        public static string GetAppSetting(string appSection, string appValue)
        {
            return SettingsReader.AppSetting(appSection, appValue);
        }

        public static string GetAppKey(string key)
        {
            return SettingsReader.AppSetting($"AppKeys:{key}", SettingsReader.AppSetting("AppKeys", "AppMode")); 
        }

        public static string WDSServer
        {
            get
            {
                return GetAppKey("WDSServer");
            }
        }
    }
}
