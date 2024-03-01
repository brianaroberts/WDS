using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataService.Core.Settings
{
    public class SettingsConfigHelper
    {
        private static SettingsConfigHelper _appSettings;

        public string AppSettingValue { get; set; }

        public static string AppSetting(string section, string key)
        {
            _appSettings = GetCurrentSettings(section, key);
            return _appSettings.AppSettingValue;
        }

        public SettingsConfigHelper(IConfiguration config, string Key)
        {
            AppSettingValue = config.GetValue<string>(Key);
        }

        public static SettingsConfigHelper GetCurrentSettings(string section, string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();

            var settings = new SettingsConfigHelper(configuration.GetSection(section), key);

            return settings;
        }

        public static string GetAppSetting(string appSection, string appValue)
        {
            return AppSetting(appSection, appValue);
        }
    }
}
