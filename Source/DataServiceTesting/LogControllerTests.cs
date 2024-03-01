using Microsoft.AspNetCore.Mvc;
using WDS.Controllers;
using static WDSClient.Models.WDSEnums;

namespace DataServiceTesting
{
    public class LogControllerTests
    {
        public static LogController? controller;        

        [SetUp]
        public void Setup()
        {
            controller = new LogController();
        }

        [Test]
        public void TestC2IEDailyCoalition_GetData()
        {
            ActionResult result = (ActionResult)controller.Log("clientHost", "application", "area", "Log message", LogLevels.Information);
            Assert.That(result.Equals(true));
        }
    }
}