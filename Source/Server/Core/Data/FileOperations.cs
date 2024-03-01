using DataService.Core.Logging;
using DataService.Core.Settings;
using DataService.Models;
using DataService.Models.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using WDS.Core.Extensions;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Data
{
    public static class FileOperations
    {
        private static object _WDSFileLocker = new object();
        public static WDSServerResponse UpdateFile(WDSServerRequest request, bool isDatePartitioned = false, bool isHostPartitioned = false)
        {
            WDSServerResponse response = new WDSServerResponse(request);
            var parameters = request.CallParameters; 
            string pathToFile = DataServiceSettings.GetConnection(request.Application, request.Call);
            string partition = isDatePartitioned ? $"{DateTime.Now.ToString("yyyyMMdd")}\\" : "";
            partition = isHostPartitioned ? $"{parameters.GetAValue("host", "unknown")}\\" : partition; 
            
            string domain = parameters.GetAValue("domain","unknown");
            string logLevel = parameters.GetAValue("logLevel", "unknown");
            var log = new Dictionary<string, string>()
            {
                { "host", parameters.GetAValue("host", "unknown")},
                { "area", parameters.GetAValue("area", "unknown")},
                { "message", parameters.GetAValue("message", "unknown")}
                
            }; 
            
            if (pathToFile.IsNullOrEmpty())
            {
                response.ErrorMessage = $"{request.Application}-{request.Call} Did not have a path to save the file too. ";
            }
            else
            {
                pathToFile = $"{pathToFile}\\{domain}\\{partition}{logLevel}.log";
                try
                {
                    UpdateFile(pathToFile, log); 
                }
                catch (Exception ex)
                {
                    response.ErrorMessage = ex.Message;
                    WDSLogs.WriteServiceDBLog(LogLevels.Error, ex.Message);
                }
            }
            return response;
        }

        public static void UpdateFile (string pathToFile, Dictionary<string, string> data)
        {    
            lock (_WDSFileLocker)
            {
                if (!File.Exists(pathToFile))
                {
                    string directory = Path.GetDirectoryName(pathToFile);
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    var fs = File.Create(pathToFile);
                    fs.Close();
                    File.AppendAllText(pathToFile, data.GetKeys(",") + Environment.NewLine);
                }
                File.AppendAllText(pathToFile, data.GetValues(",") + Environment.NewLine);
            }             
        }

        public static void UpdateFile(string pathToFile, List<string> data)
        {
            lock (_WDSFileLocker)
            {
                if (!File.Exists(pathToFile))
                {
                    string directory = Path.GetDirectoryName(pathToFile);
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    var fs = File.Create(pathToFile);
                    fs.Close();
                }
                File.AppendAllText(pathToFile, String.Join(Environment.NewLine, data.ToArray()) + Environment.NewLine);
            }
        }

        public static WDSServerResponse GetDataFromFile(WDSServerRequest request) 
        {
            //TODO: need to implement
            return new WDSServerResponse(); 
        }
    }
}
