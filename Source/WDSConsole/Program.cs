using System.Diagnostics;
using WDS.Models;
using WDSClient;

var myUserId = "0000000";
var myADUserName = "DOMAIN\\robertsba";

var wdsClient = new Client(new WDSClientReader(myUserId));

Console.WriteLine($"Loading user...{myUserId}"); 
Console.WriteLine(wdsClient.User.ToString());
if (wdsClient.User.Roles != null)
{
    foreach (var role in wdsClient.User.Roles)
    {
        Console.WriteLine($"{role.Role}-");
    }
    Console.WriteLine();
}

while (true)
{
    var _executionTimer = new Stopwatch();
    _executionTimer.Start();
    wdsClient.WriteLog("Begining", "Starting to write", WDSClient.Models.LogLevels.Information)
        .AddCall("Countries", "", DataFormatTypes.CSV)        
        .AddCall("DimDatesCSV", "Day_Name=Friday")
        .AddCall("EVehiclePopulation", "numRows=19999")
        .WriteLog("Middle", "Before we make an Sync", WDSClient.Models.LogLevels.Error);

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(wdsClient.GetRequestBody());
    Console.ForegroundColor = ConsoleColor.White;
    wdsClient.SendAsync();

    Console.WriteLine($"Successfully completed {wdsClient.Responses.Count} requests.");

    foreach (var response in wdsClient.Responses.Values)
    {
        if (response.ContainsError)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"REQ_ID:{response.Request.Id} contained error {response.ErrorMessage}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{response.Request.Id} returned in {response.ExecutionDuration} on {response.ExecutionDate}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            var rd = response.GetData();
            //if (rd != null)
            //    Console.WriteLine(response.ReturnValue);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    _executionTimer.Stop();
    Console.WriteLine(_executionTimer.Elapsed.ToString());
    Console.WriteLine("Press any key to run again."); 
    Console.ReadKey();
}



