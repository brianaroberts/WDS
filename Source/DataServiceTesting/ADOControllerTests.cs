using DataService.Controllers;
using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Xml.Linq;

namespace DataServiceTesting
{
    public class ADOControllerTests
    {
        public static ADOController? adoController;

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
            adoController = new ADOController();
            DataServiceSettings.Load(hosting, $"{Directory.GetCurrentDirectory()}\\settings");
        }


        //[Test]
        //public void TestGetProjects()
        //{
        //    ViewResult actionResult = (ViewResult)adoController.GetProjects();
        //    Assert.That(actionResult.ViewData.Count, Is.GreaterThan(0));
        //}
    }
}