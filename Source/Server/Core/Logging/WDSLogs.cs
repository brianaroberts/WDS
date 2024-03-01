using DataService.Core.Data;
using WDS.Core.Extensions;
using DataService.Models.Settings;
using System;
using System.Collections.Generic;
using DataService.Models;
using System.Linq;
using DataService.Core.Settings;
using WDSClient.Models;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Logging
{


    public class WDSLogModel
    {
        public WDSServerRequest Request { get; }
        public string ExecutionDate { get; }
        public string ExecutionDuration { get; }
        public string SQLExecutionDuration { get; }


        public WDSLogModel(WDSServerResponse logResponse)
        {
            Request = logResponse.Request;
            ExecutionDate = logResponse.ExecutionDate;
            ExecutionDuration = logResponse.ExecutionDuration;  
            SQLExecutionDuration = logResponse.SQLExecutionDuration;    
        }
    }

    public static class WDSLogs
    {
        public static string WDS_LOG_CONNECTION_ID = "WDSLogging"; 
        private static string _pathToLog;
        private static int _batchLogCount = 1; 
        private static readonly List<WDSLogModel> _recordsToSend = new List<WDSLogModel>();
        private static readonly List<string> _messagesToSend = new List<string>();

        public static string PathToLog
        {
            get
            {
                if (_pathToLog.IsNullOrEmpty())
                {
                    if (DataServiceSettings.Connections.ConnectionByName.ContainsKey(WDS_LOG_CONNECTION_ID))
                        _pathToLog = DataServiceSettings.Connections.ConnectionByName[WDS_LOG_CONNECTION_ID].GetConnection();
                    if (_pathToLog.IsNullOrEmpty())
                    {
                        return "C:\\WDS_Logs"; 
                    }
                }
                return _pathToLog;
            }
        }
        
        public static void AddLog(WDSServerResponse logResponse)
        {
            _recordsToSend.Add(new WDSLogModel(logResponse));
            if (_recordsToSend.Count >= _batchLogCount)
                Send(); 
        }

        public static void AddLog(string message, LogLevels logLevel)
        {
            if (DataServiceSettings.LogLevel <= LogLevels.Information)
            {
                _messagesToSend.Add($"{DateTime.Now} - {message}");            
                Send();
            }
               
        }
        public static void Send()
        {
            //TODO: Go through the UserParameters and all data to pull in what is needed for the log. 
            foreach (var record in _recordsToSend.Where(o=>o.Request != null))
            {
                var log = new Dictionary<string, string>()
                {
                    { "Application", record.Request.Application },
                    { "CallType", record.Request.CallType.ToString() },
                    { "ExecutionDate", record.ExecutionDate },
                    { "ExecutionDuration", record.ExecutionDuration },
                    { "SQLExecutionDuration", record.SQLExecutionDuration },
                    { "Host", record.Request.Host },
                    { "ConnectionName", record.Request.GetConnectionName().ToString() },
                    { "Statement", record.Request.Statement },
                    { "UserParameters", record.Request.UserParameters.ToString(";") },
                };
                FileOperations.UpdateFile($"{PathToLog}\\{DateTime.Now.ToString("yyyyMMdd")}_WDSCalls.log", log);  
            }
            _recordsToSend.Clear();

            FileOperations.UpdateFile($"{PathToLog}\\{DateTime.Now.ToString("yyyyMMdd")}_WDS.log", _messagesToSend);
            _messagesToSend.Clear();
        }

        public static void WriteServiceDBLog(LogLevels logLevel, string message)
        {
            if (DataServiceSettings.LogLevel <= logLevel)
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("instanceId", DataServiceSettings.InstanceGuid);
                parameters.Add("logLevel", ((int)logLevel).ToString());
                parameters.Add("host", DataServiceSettings.Connections.HostName);
                parameters.Add("message", message);
                string sqlStatement = $"[WDS].[add_serviceLog]";
                WDSWriteOperations.UpdateDB(sqlStatement, parameters);
            }
        }
    }
}
