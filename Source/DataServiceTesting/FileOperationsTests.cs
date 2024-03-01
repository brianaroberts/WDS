using DataService.Controllers;
using DataService.Core.Data;
using DataService.Core.Settings;
using DataService.Models;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System.Xml.Linq;
using static WDSClient.Models.WDSEnums;

namespace DataServiceTesting
{
    public class FileOperationsTests
    {
        WDSServerRequest _request = new WDSServerRequest();

        [SetUp]
        public void Setup()
        {
            _request.Application = "ParkingPortal";
            _request.Statement = "PP statement";
            _request.Call = "sp_call";
            _request.CallParameters = new Dictionary<string, string>();
            _request.CallType = CallTypes.StoredProc;
            _request.Host = "host";
        }

        [Test]
        public void TestUpdateFile()
        {
            try
            {
                FileOperations.UpdateFile("c:\\source\\file.txt", new Dictionary<string, string>());
                Assert.Pass();
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TestUpdateFile2()
        {
            WDSServerResponse response = FileOperations.UpdateFile(_request, false, false);
            Assert.That(response.ReturnValue, Is.True);
        }

        [Test]
        public void TestGetDataFromFile()
        {
            WDSServerResponse response = FileOperations.GetDataFromFile(_request);
            Assert.That(response.ReturnValue, Is.Empty);
        }
    }
}