using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace DataServiceTesting
{
    public class ApplicationTests
    {
        public static Application? controller;

        internal class HostingEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Development";

            public string? ApplicationName { get; set; } = "WDS";

            public string? WebRootPath { get; set; } = "C:\\\\Source\\\\WDS\\\\Source\\\\Server\\\\";

            public IFileProvider? WebRootFileProvider { get; set; }

            public string? ContentRootPath { get; set; } = "C:\\Source\\WDS\\Source\\Server";

            public IFileProvider? ContentRootFileProvider { get; set; }
        }

        List<string> roles = new()
            {
                "Admin"
            };

        [SetUp]
        public void Setup()
        {
            HostingEnvironment hosting = new HostingEnvironment();
            controller = new Application(new WDS.Settings.ApplicationModel() 
            {
               Name = "Test App",
               DataOwner = "Paul D",
               Description = "An application for testing"
            }, true);
            
        }

        [Test]
        public void TestAddRecordset()
        {
            bool value = controller.AddRecordset(new RecordsetModel() 
                {
                    Application = "Test App", 
                    Name = "Test", 
                    ConnectionName = "Test",
                    CallType = WDSClient.Models.WDSEnums.CallTypes.NotSet, 
                    CacheTimeOut = 0,
                    ReturnType = WDSClient.Models.WDSEnums.DataFormatTypes.Json, 
                    DataDetailType = WDSClient.Models.WDSEnums.DataDetailTypes.Default,
                    Statement = "[wds].[test]",
                    LogCallInDB = true,
                    Role = "Test.Read"
                }, true);
            Assert.That(value, Is.True);
        }

        [Test]
        public void TestGetApplicationsFromDB()
        {
            Dictionary<string, Application> apps = Application.GetApplicationsFromDB(new Microsoft.Data.SqlClient.SqlConnection("{ConnectionString}"));
            Assert.That(apps.Count, Is.GreaterThan(0));
        }
    }
}