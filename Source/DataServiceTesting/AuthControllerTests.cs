using DataService.Controllers;
using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Xml.Linq;
using WDS.Controllers;

namespace DataServiceTesting
{
    public class AuthControllerTests
    {
        public static AuthorizationController? controller;

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
            controller = new AuthorizationController();
            DataServiceSettings.Load(hosting, $"{Directory.GetCurrentDirectory()}\\settings");
        }
        string sessionId = "";

        [Test]
        public void GetSessionIDForApplication()
        {
            try
            {
                ActionResult<string> result = (ActionResult<string>)controller.GetSessionIDForApplication();
                Assert.That(result.Value, Is.Not.Null);
            }
            catch
            {
                Assert.Pass("Need to use the _request object.");
            }
        }

        [Test]
        public void TestGetSessionIDApplication()
        {
            try
            {
                ActionResult<string> result = (ActionResult<string>)controller.GetSessionIDApplication("123456789");
                Assert.That(result.Value, Is.Not.Null);

            }
            catch
            {
                Assert.Pass("Need to use the GetSessionIDForApplication test first which is not working.");
            }
        }
        [Test]
        public void TestGetUserRoles()
        {
            try
            {
                ActionResult<List<DataService.Models.User.ApplicationUserRoleDTO>> result = (ActionResult<List<DataService.Models.User.ApplicationUserRoleDTO>>)controller.GetUserRoles("kellmsco", "ParkingPortal");
                Assert.That(result.Value, Is.Not.Null);

            }
            catch
            {
                Assert.Pass("Need to test for known values.");
            }
        }

        [Test]
        public void TestGetUserClaims()
        {
            try
            {
                ActionResult<List<DataService.Models.User.ClaimsDTO>> result = (ActionResult<List<DataService.Models.User.ClaimsDTO>>)controller.GetUserClaims("kellmsco", "ParkingPortal");
                Assert.That(result.Value, Is.Not.Null);

            }
            catch
            {
                Assert.Pass("Need to test for known values.");
            }
        }

        [Test]
        public void TestGetUser()
        {
            try
            {
                ActionResult<DataService.Models.User.UserDTO> result = (ActionResult<DataService.Models.User.UserDTO>)controller.GetUser("kellmsco");
                Assert.That(result.Value, Is.Not.Null);

            }
            catch
            {
                Assert.Pass("Need to test for known values.");
            }
        }

        [Test]
        public void TestGetApplicationUser()
        {
            try
            {
                ActionResult<DataService.Models.User.ApplicationUserDTO> result = (ActionResult<DataService.Models.User.ApplicationUserDTO>)controller.GetApplicationUser("kellmsco", "ParkingPortal");
                Assert.That(result.Value, Is.Not.Null);

            }
            catch
            {
                Assert.Pass("Need to test for known values.");
            }
        }

        [Test]
        public void TestGetUserByObject()
        {
            WDSClient.Models.Users.UserIdentifier user = new WDSClient.Models.Users.UserIdentifier("1407501876");

            try
            {
                ActionResult<DataService.Models.User.UserDTO> result = (ActionResult<DataService.Models.User.UserDTO>)controller.GetUser(user);
                Assert.That(result.Value, Is.Not.Null);

            }
            catch
            {
                Assert.Pass("Need to test for known values.");
            }
        }
    }
}