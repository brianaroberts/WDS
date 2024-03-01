using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using WDS.Settings;

namespace DataServiceTesting
{
    public class ConnectionsTests
    {
        public static Connections? controller;

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
            controller = new Connections();
            //DataServiceSettings.Load(hosting, $"{Directory.GetCurrentDirectory()}\\settings");
        }

        //[Test]
        //public void TestGetConnectionIDFromDB()
        //{
        //    int value = controller.GetConnectionIDFromDB("WDS");
        //    Assert.That(value, Is.GreaterThan(0));
        //}

        [Test]
        public void TestConnectionByName()
        {
            Dictionary<string, ConnectionModel> value = controller.ConnectionByName;
            Assert.That(value.Count, Is.GreaterThan(0));
        }

        [Test]
        public void TestRefreshDatabaseConnections()
        {
            try
            {
                controller.RefreshDatabaseConnections("connection string", true);
                Assert.Pass();
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestSetupConnections()
        {
            try
            {
                controller.SetupConnections("c:\\source\\test.txt");
                Assert.Pass();
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestModifyConnectionToDB()
        {
            int value = controller.UpdateConnectionToDB(new ConnectionModel());
            Assert.That(value, Is.GreaterThan(0));
        }

        //[Test]
        //public void TestGetConnectionIDFromDBByName()
        //{
        //    int value = controller.GetConnectionIDFromDB("WDS");
        //    Assert.That(value, Is.GreaterThan(0));
        //}

        [Test]
        public void TestSetRunningEnvironment()
        {
            HostingEnvironment hosting = new HostingEnvironment();

            try
            {
                controller.SetRunningEnvironment(hosting);
                Assert.Pass();
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}