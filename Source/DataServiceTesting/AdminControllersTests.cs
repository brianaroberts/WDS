using DataService.Controllers;
using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Collections;
using WDS.Authentication;
using WDS.Settings;

namespace DataServiceTesting
{
    public class AdminControllerTests
    {
        public static AdminController? adminController;

        internal class HostingEnvironment : IWebHostEnvironment
        {
            public string EnvironmentName { get; set; } = "Development";

            public string? ApplicationName { get; set; } = "WDS";

            public string? WebRootPath { get; set; } = "C:\\\\Source\\\\WDS\\\\Source\\\\Server\\\\";

            public IFileProvider? WebRootFileProvider { get; set; }

            public string? ContentRootPath { get; set; } = "C:\\Source\\WDS\\Source\\Server";

            public IFileProvider? ContentRootFileProvider { get; set; }
        }

        internal class ServiceCollection : IServiceCollection
        {
            public ServiceDescriptor this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public int Count => throw new NotImplementedException();

            public bool IsReadOnly => throw new NotImplementedException();

            public void Add(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<ServiceDescriptor> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public int IndexOf(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void Insert(int index, ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public bool Remove(ServiceDescriptor item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(int index)
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        List<string> roles = new()
            {
                "Admin"
            };

        [SetUp]
        public void Setup()
        {
            HostingEnvironment hosting = new HostingEnvironment();
            adminController = new AdminController();
            DataServiceSettings.Load(hosting, $"{Directory.GetCurrentDirectory()}\\settings");
        }


        [Test]
        public void TestEncrypt()
        {
            ObjectResult actionResult = (ObjectResult)adminController.Encrypt("12345", "12345");
            Assert.That(actionResult.Value, Is.EqualTo("aWB0O/TSxUyJc4IeR7aZGw=="));
        }

        [Test]
        public void TestDecrypt()
        {
            ObjectResult actionResult = (ObjectResult)adminController.Decrypt("12345", "aWB0O/TSxUyJc4IeR7aZGw==");
            Assert.That(actionResult.Value, Is.EqualTo("12345"));
        }

        [Test]
        public void TestGenerateGuid()
        {
            Guid guidResult;

            ObjectResult actionResult = (ObjectResult)adminController.GenerateGuid();

            _ = Guid.TryParse((string?)actionResult.Value.ToString(), out guidResult);
        }

        [Test]
        public void TestRestoreAPIKeys()
        {
            ObjectResult actionResult = (ObjectResult)adminController.RestoreAPIKeys(DateTime.Now.ToString());
            Assert.That(actionResult.Value, Is.False);
        }


        [Test]
        public void TestRestoreApp()
        {
            ObjectResult actionResult = (ObjectResult)adminController.RestoreApp(DateTime.Now.ToString(), "WDS");
            Assert.That(actionResult.Value, Is.False);
        }


        [Test]
        public void TestCreateAPIKey()
        {
            Guid guidResult;
            var guid = DataServiceSettings.ApiKeys.AddAPIKey("NewKey", "brianaroberts@live.com", "1.2.3.4", "TEST TEST TEST", roles);
            _ = Guid.TryParse((string?)guid.ToString(), out guidResult);
        }

        [Test]
        public void TestGetAPIKey()
        {
            var guidResult = DataServiceSettings.ApiKeys.AddAPIKey("TestAPIKey", "owner@email.com", "*", "none"); 

            ObjectResult actionResult = (ObjectResult)adminController.GetAPIKey("TestAPIKey");
            Assert.AreEqual(guidResult, ((ApiKeyModel)actionResult.Value).ApiKey); 
        }

        [Test]
        public void TestGetApplication()
        {
            ObjectResult actionResult = (ObjectResult)adminController.GetApplication("ParkingPortal");
            var application = (ApplicationModel)actionResult.Value;
            Assert.That(application.Name, Is.EqualTo("ParkingPortal"));
        }

        [Test]
        public void TestAddApplication()
        {
            List<ParameterModel> parameters = new List<ParameterModel>();
            ParameterModel parameter = new ParameterModel();
            parameter.DefaultValue = DateTime.Now.ToString();
            parameter.RegularExpression = "";
            parameter.RecordsetParameter = "@recordsetName";
            parameter.Name = "@date";
            parameter.Required = false;

            parameters.Add(parameter);
            RecordsetModel recordset = new RecordsetModel();
            recordset.Application = "Scott";
            recordset.Statement = "Statement";
            recordset.Parameters = parameters;


            List<RecordsetModel> recordsets = new List<RecordsetModel>();
            recordsets.Add(recordset);


            WDS.Settings.ApplicationModel application = new WDS.Settings.ApplicationModel();
            application.Name = "Scott";
            application.DataOwner = "brianaroberts@live.com";
            application.Recordsets = recordsets;
            application.Description = "Description";


            ObjectResult actionResult = (ObjectResult)adminController.AddApplication(application, true);

            Assert.That(application.Name, Is.EqualTo("Scott"));
        }

        [Test]
        public void TestAddRecordsetToApplication()
        {
            List<ParameterModel> parameters = new List<ParameterModel>();
            ParameterModel parameter = new ParameterModel();
            parameter.DefaultValue = DateTime.Now.ToString();
            parameter.RegularExpression = "";
            parameter.RecordsetParameter = "@date";
            parameter.Name = "@date";
            parameter.Required = false;

            parameters.Add(parameter);
            RecordsetModel recordset = new RecordsetModel();
            recordset.Name = "ParkingPortal";
            recordset.Statement = "Statement";
            recordset.Parameters = parameters;


            ObjectResult actionResult = (ObjectResult)adminController.AddRecordsetToApplication("ParkingPortal", recordset);

            Assert.That(actionResult.Value, Is.True);
        }

        [Test]
        public void TestAddParameterToRecordSet()
        {
            List<ParameterModel> parameters = new List<ParameterModel>();
            ParameterModel parameter = new ParameterModel();
            parameter.DefaultValue = DateTime.Now.ToString();
            parameter.RegularExpression = "";
            parameter.RecordsetParameter = "@recordsetName";
            parameter.Name = "date";
            parameter.Required = false;

            parameters.Add(parameter);
            RecordsetModel recordset = new RecordsetModel();
            recordset.Application = "ParkingPortal";
            recordset.Statement = "Statement";
            recordset.Parameters = parameters;


            List<RecordsetModel> recordsets = new List<RecordsetModel>();
            recordsets.Add(recordset);


            WDS.Settings.ApplicationModel application = new WDS.Settings.ApplicationModel();
            application.Name = "Scott";
            application.DataOwner = "brianaroberts@live.com";
            application.Recordsets = recordsets;
            application.Description = "Description";


            ObjectResult actionResult = (ObjectResult)adminController.AddParameterToRecordSet(application.Name, parameter.Name, parameter);

            Assert.That(actionResult.Value, Is.True);
        }


        [Test]
        public void TestAddUserToApplication()
        {
            List<ParameterModel> parameters = new List<ParameterModel>();
            ParameterModel parameter = new ParameterModel();
            parameter.DefaultValue = DateTime.Now.ToString();
            parameter.RegularExpression = "";
            parameter.RecordsetParameter = "@recordsetName";
            parameter.Name = "date";
            parameter.Required = false;

            parameters.Add(parameter);
            RecordsetModel recordset = new RecordsetModel();
            recordset.Application = "ParkingPortal";
            recordset.Statement = "Statement";
            recordset.Parameters = parameters;


            List<RecordsetModel> recordsets = new List<RecordsetModel>();
            recordsets.Add(recordset);


            WDS.Settings.ApplicationModel application = new WDS.Settings.ApplicationModel();
            application.Name = "Scott";
            application.DataOwner = "brianaroberts@live.com";
            application.Recordsets = recordsets;
            application.Description = "Description";


            ObjectResult actionResult = (ObjectResult)adminController.AddUserToApplication(1407501876, '1', application.Name, "Admin");

            Assert.That(actionResult.Value, Is.True);
        }

        [Test]
        public void TestRefreshConnections()
        {
            ObjectResult actionResult = (ObjectResult)adminController.RefreshConnections();
            Assert.That(actionResult.Value, Is.True);
        }

        [Test]
        public void TestRestoreConnections()
        {
            adminController.BackupSettings(); 

            ObjectResult actionResult = (ObjectResult)adminController.RestoreConnections(DateTime.Now.ToString("yyyy.MM.dd"));
            Assert.That(actionResult.Value, Is.True);
        }

        [Test]
        public void TestModifyServiceVariable()
        {
            ObjectResult actionResult = (ObjectResult)adminController.ModifyServiceVariable("name", "value", "description", "hostName");
            Assert.That(actionResult.Value, Is.True);
        }

        [Test]
        public void TestGetServiceVariables()
        {
            ObjectResult actionResult = (ObjectResult)adminController.GetServiceVariables();
            Assert.That((int)actionResult.Value, Is.GreaterThan(0));
        }

        [Test]
        public void TestBackupSettings()
        {
            ObjectResult actionResult = (ObjectResult)adminController.BackupSettings();
            Assert.That(actionResult.Value, Is.True);
        }
    }
}