using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WDS.Authentication;
using WDS.Controllers;

namespace DataServiceTesting
{
    public class APIKeysModelTests
    {
        public ApiKeysModel model;
        public ApiKeys _apiKeys;
        public string _apiKey = "ef07bcb2-05d5-49cb-bd86-78320e045a6e"; 

        [SetUp]
        public void Setup()
        {
            var apiKey = new ApiKeyModel("ParkingPortal", _apiKey, "owner@email.com", "1.2.3.4", new List<string>() { "WDS.Read", "WDS.Write", "WDS.Admin" });
            model = new ApiKeysModel();
            model.Keys.Add(apiKey);
            _apiKeys = new ApiKeys(model); 
        }

        [Test]
        public void TestRemoveAPIKey()
        {
            try
            {
                _apiKeys.RemoveAPIKey("Scott");
                Assert.That(true);
            }
            catch
            {
                Assert.That(false);
            }
        }

        [Test]
        public void TestDoesKeyExist()
        {
            bool resultScott = _apiKeys.DoesKeyExist("Scott");
            Assert.That(resultScott, Is.False);

            
            bool resultPP = _apiKeys.DoesKeyExist(_apiKey);
            Assert.That(resultPP, Is.True);
        }

        [Test]
        public void TestRetrieveKey()
        {
            ApiKeyModel apiKeyModel = _apiKeys.RetrieveKeyByName("ParkingPortal");
            var apiKey = _apiKeys.RetrieveKey(apiKeyModel.ApiKey);

            Assert.AreEqual(apiKeyModel, apiKey);
        }

        [Test]
        public void TestRetrieveKeyByName()
        {
            ApiKeyModel apiKeyModel = _apiKeys.RetrieveKeyByName("ParkingPortal");
            Assert.That(apiKeyModel.Name, Is.EqualTo("ParkingPortal"));
        }

        [Test]
        public void TestGetIPMask()
        {
            ApiKeyModel apiKeyModel = _apiKeys.RetrieveKeyByName("ParkingPortal");
            string apiMask = apiKeyModel.IPMask; 
            Assert.That(apiMask, Is.EqualTo("1.2.3.4"));
        }
    }
}