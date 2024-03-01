using DataService.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using WDS.Settings;

namespace DataServiceTesting
{
    public class DataServiceSettingsTests
    {
        internal class HostingEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Development";

            public string? ApplicationName { get; set; } = "WDS";

            public string? WebRootPath { get; set; } = "C:\\Source\\WDS\\Source\\Server\\";

            public IFileProvider? WebRootFileProvider { get; set; }

            public string? ContentRootPath { get; set; } = "C:\\Source\\WDS\\Source\\Server";

            public IFileProvider? ContentRootFileProvider { get; set; }
        }

        readonly List<string> roles = new()
            {
                "Admin"
            };

        [SetUp]
        public void Setup()
        {
            HostingEnvironment hosting = new HostingEnvironment();
            DataServiceSettings.Load(hosting, $"{Directory.GetCurrentDirectory()}\\settings");
        }

        [Test]
        public void TestGetConnection()
        {
            string connection = DataServiceSettings.GetConnection("ParkingPortal", "GetSpaces");
            Assert.That(connection, Is.Not.Null);
        }

        [Test]
        public void TestRestoreConnections()
        {
            bool result = DataServiceSettings.RestoreConnections(DateTime.Now.ToString());
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestAddUser()
        {
            bool result = DataServiceSettings.AddUser(1407501876, 'A', "ParkingPortal", "Admin");
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestBackup()
        {
            bool result = DataServiceSettings.Backup();
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestGetApplicationFromJson()
        {
            ApplicationModel result = (ApplicationModel)DataServiceSettings.GetApplicationFromJson("name", "json");
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestRestoreApp()
        {
            bool result = DataServiceSettings.RestoreApp(DateTime.Now.ToString(), "ParkingPortal");
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestRestoreAPIKeys()
        {
            bool result = DataServiceSettings.RestoreAPIKeys(DateTime.Now.ToString());
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestContainsAppRecordset()
        {
            Recordset result = DataServiceSettings.Rs("ParkingPortal", "recordset");
            Assert.That(result, Is.True);
        }
    }
}