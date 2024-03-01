using DataService.Core.Data;
using DataService.Models;
using DataService.Models.Settings;
using System.Data;
using WDS.Data;

namespace DataServiceTesting
{
    public class GetDataTests
    {
        //WDSServerRequest _request = new WDSServerRequest();

        //[SetUp]
        //public void Setup()
        //{
        //    _request.Application = "ParkingPortal";
        //    _request.Statement = "PP statement";
        //    _request.Call = "sp_call";
        //    _request.CallParameters = new Dictionary<string, string>();
        //    _request.CallType = CallTypes.StoredProc;
        //    _request.Host = "host";
        //    _request.RequestedDataFormatType = WDS.Models.DataFormatTypes.Json;
        //}

        //[Test]
        //public void TestFromSQLServe1r()
        //{
        //    try
        //    {
        //        SQLHelpers.FromSQLServer<Task>(_request);
        //        Assert.Pass();
        //    }
        //    catch
        //    {
        //        Assert.Fail();
        //    }
        //}


        //[Test]
        //public void TestFromSQLServer2()
        //{
        //    try
        //    {
        //        SQLHelpers.FromSQLServer(_request);
        //        Assert.Pass();
        //    }
        //    catch
        //    {
        //        Assert.Fail();
        //    }
        //}

        //[Test]
        //public void TestGetCommandType()
        //{
        //    try
        //    {
        //        SQLHelpers.GetCommandType(_request);
        //        Assert.Pass();
        //    }
        //    catch
        //    {
        //        Assert.Fail();
        //    }
        //}

        //[Test]
        //public void TestGetCachedVersion()
        //{
        //    WDSServerResponse response = SQLHelpers.GetCachedVersion("c:\\source\\text.txt");
        //    Assert.That(response.ContainsError, Is.False);
        //}

        //[Test]
        //public void TestSaveCachedVersion()
        //{
        //    try
        //    {
        //        WDSServerResponse response = SQLHelpers.GetCachedVersion("c:\\source\\text.txt");
        //        SQLHelpers.SaveCachedVersion(response);
        //        Assert.Pass("SaveCachedVersion did not throw any errors.");
        //    }
        //    catch
        //    {
        //        Assert.Fail();
        //    }
        //}
    }
}