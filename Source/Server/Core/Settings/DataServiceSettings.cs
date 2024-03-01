using WDS.Core.Extensions;
using DataService.Models.Settings;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using DataService.Core.Data;
using WDS.Settings;
using DataService.Models;
using System.Collections;
using WDSClient.Models;
using WDSClient;
using DataService.Core.Logging;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Settings
{
    public static class DataServiceSettings
    {
        
        public static string EmailTemplatesDirectory;
        private static object _lockObj = new object(); 
        public static string InstanceGuid { get;  } = Guid.NewGuid().ToString();
        public static Dictionary<string, Application> Applications = new Dictionary<string, Application>(StringComparer.OrdinalIgnoreCase);
        public static Dictionary<string, string> SystemEnvVariables = new Dictionary<string, string>();
        public static Dictionary<string, string> WDSServiceVariables = new Dictionary<string, string>();
        public static Connections Connections = new Connections();
        public static ApiKeys ApiKeys = new ApiKeys();
        public static LogLevels LogLevel { private set; get; }
        public static string WDSConnectionString
        {
            get
            {   
                return Connections.ConnectionByName.ContainsKey("WDS") ? Connections.ConnectionByName["WDS"]?.GetConnection() : String.Empty;
            }
        }
        public static void LoadApplicationsFromDB()
        {
            lock (_lockObj)
            {
                WDSLogs.WriteServiceDBLog(LogLevels.Information, $"Loading DataService Settings from database {WDSConnectionString}.");
                using (SqlConnection sqlConnection = new SqlConnection(WDSConnectionString))
                {
                    // TODO: Modify the section below after selecting an appropriate MicroORM
                    sqlConnection.Open();

                    var applicationsFromDB = Application.GetApplicationsFromDB(sqlConnection);
                    foreach( var application in applicationsFromDB.Where(o=> (!Applications.ContainsKey(o.Key) || Applications[o.Key]?.Model?.SourcedFrom == SourcedFromTypes.Database)))
                    {
                        if (!Applications.ContainsKey(application.Key))
                            Applications.Add(application.Key, application.Value);
                        else
                            Applications[application.Key] = application.Value;
                    }
                    GetServiceVariablesFromDB();

                    sqlConnection.Close();
                }
            }
        }
        public static void Load(IWebHostEnvironment env, string directoryPath = "", bool loadDB = true)
        {
            
            var directory = new DirectoryInfo($"{directoryPath}//Applications");
            SystemEnvVariables = Environment.GetEnvironmentVariables().Cast<DictionaryEntry>().ToDictionary(kvp=>kvp.Key.ToString(), kvp=>kvp.Value.ToString());
            if (int.TryParse(SettingsReader.GetAppSetting("AppKeys", "LogLevel"), out int _logLevel) && _logLevel >= 0)
                LogLevel = (LogLevels)_logLevel; 

            if (!directory.Exists)
                return; // Directory not setup. 

            var applicationFiles = directory.GetFiles("*.json");
            EmailTemplatesDirectory = $"{directoryPath}\\EmailTemplates";

            Connections.SetRunningEnvironment(env);

            Connections.SetupConnections(directoryPath);
            WDSLogs.WriteServiceDBLog(LogLevels.Information, $"Loading DataService Settings from {directoryPath}");

            // This will replace any applications that are currently in the object. 
            if (loadDB) LoadApplicationsFromDB();

            lock (_lockObj)
            {
                ApiKeys.Load(directoryPath, DataServiceSettings.WDSConnectionString);
                ApiKeys.Model.Keys.ForEach(k => k.SourcedFrom = SourcedFromTypes.Database); 

                // LOCAL .json files will overwrite anything in db
                foreach (var settingsFile in applicationFiles.Select(o => o))
                {
                    var applicationName = settingsFile.Name.Replace(settingsFile.Extension, "");

                    if (!Applications.ContainsKey(applicationName))
                        Applications.Add(applicationName, null);

                    try
                    {
                        var app = GetApplicationFromJson(applicationName, File.ReadAllText(settingsFile.FullName));
                        app.SourcedFrom = SourcedFromTypes.File;
                        Applications[applicationName] = new Application(app, true);  
                        
                    }
                    catch (Exception ex)
                    {
                        var temp = ex.Message;
                        WDSLogs.WriteServiceDBLog(LogLevels.Error, $"DataServiceSettings.Load: {ex.Message}");
                    }                    
                }
            }
        }
        public static string GetConnection(string application, string recordset)
        {
            var connectionId = Applications.ContainsKey(application) ?
                Applications[application].Recordsets.ContainsKey(recordset) ? Applications[application].Recordsets[recordset].Model.ConnectionName : String.Empty
                : String.Empty;

            return Connections.ConnectionByName[connectionId].GetConnection();
        }
        private static void GetServiceVariablesFromDB()
        {
            using (SqlConnection sqlConnection = new SqlConnection(WDSConnectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand($"SELECT var_name, var_value from [wds].[service_variables] WHERE is_active = 1 AND expire_date > getdate() AND server_host_id IN (SELECT host_id FROM [wds].[hosts] WHERE host_name = '{DataServiceSettings.Connections.HostName}')", sqlConnection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable objTable = new DataTable();
                    adapter.Fill(objTable);

                    foreach (DataRow row in objTable.Rows)
                    {
                        WDSServiceVariables[row["var_name"].ToString()] = row["var_value"].ToString();
                    }

                }
                sqlConnection.Close();
            }
        }
        public static bool ModifyServiceVariable(string name, string value, string description, string hostName)
        {
            var returnValue = true;

            var parameters = new Dictionary<string, string>();
            parameters.Add("hostName", DataServiceSettings.Connections.HostName);
            parameters.Add("variableName", name);
            parameters.Add("variableValue", value);
            parameters.Add("description", description);

            string sqlStatement = $"EXEC [wds].[up_service_variable] @hostName, @variableName, @variableValue, @description";
            WDSWriteOperations.UpdateDB(sqlStatement, parameters);

            WDSServiceVariables[name] = value; 

            return returnValue; 
        }
        public static RecordsetModel GetRecordsetModel(string plugIn, string recordset)
        {
            RecordsetModel returnValue = null;

            if (Applications.ContainsKey(plugIn) && Applications[plugIn].Recordsets.ContainsKey(recordset))
                returnValue = Applications[plugIn].Recordsets[recordset].Model;

            return returnValue;
        }
        public static ApplicationModel GetApplicationFromJson(string name, string json)
        {
            ApplicationModel appModel = null;
            
            try
            {
                appModel = JsonConvert.DeserializeObject<ApplicationModel>(json);
                if (!name.IsNullOrEmpty())
                    appModel.Name = name;

                // Remove any duplicates pulled in from the Json file
                appModel.Recordsets = appModel.Recordsets.GroupBy(x=>x.Name).Select(x=>x.First()).ToList();

                foreach (var recordset in appModel.Recordsets)
                {
                    recordset.Application = name;
                }
            }
            catch (Exception ex)
            {
                var temp = ex.Message;
                WDSLogs.WriteServiceDBLog(LogLevels.Error, $"GetApplicationFromJson: {temp}. ");
            }

            return appModel;
        }
        public static bool AddUser(int edipi, char ptc, string applicationName, string roleName)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("edipi", edipi.ToString());
            parameters.Add("ptc", ptc.ToString());
            parameters.Add("appName", applicationName);
            parameters.Add("roleName", roleName);

            WDSWriteOperations.UpdateDB($"EXEC [wds].[add_user]", parameters);

            return true;
        }
        // Backs Up all the settings from the WDS to the Cache Folder
        public static bool Backup()
        {
            var returnValue = false; 

            if (DataServiceSettings.Applications != null && DataServiceSettings.Applications.Count > 0)
            {
                try
                {
                    string directory = $"{DataServiceSettings.Connections.ConnectionByName[WDSCache.WD_CACHE_ID].GetConnection()}\\settingsBackup\\{DateTime.Now.ToString("yyyy.MM.dd")}";
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    // Connections
                    var filePath = $"{directory}\\connections.{WDSCache.WD_CACHE_FILE_EXTENSION}";
                    string jsonString = JsonConvert.SerializeObject(DataServiceSettings.Connections);
                    File.WriteAllText(filePath, AesOperation.EncryptString(WDSCache.CACHED_DATA_ENCRYPT_KEY, jsonString));

                    // API Keys
                    filePath = $"{directory}\\APIKeys.{WDSCache.WD_CACHE_FILE_EXTENSION}";
                    jsonString = JsonConvert.SerializeObject(DataServiceSettings.ApiKeys);
                    File.WriteAllText(filePath, AesOperation.EncryptString(WDSCache.CACHED_DATA_ENCRYPT_KEY, jsonString));

                    foreach (var app in DataServiceSettings.Applications)
                    {
                        filePath = $"{directory}\\{app.Value.Name}.{WDSCache.WD_CACHE_FILE_EXTENSION}";
                        jsonString = JsonConvert.SerializeObject(app.Value);
                        // file will be overwritten if it exists
                        File.WriteAllText(filePath, AesOperation.EncryptString(WDSCache.CACHED_DATA_ENCRYPT_KEY, jsonString));
                        returnValue = true;
                    }

                    

                } catch (Exception ex)
                {
                    returnValue = false;
                    WDSLogs.WriteServiceDBLog(LogLevels.Error, $"Backing up settings: {ex.Message}");
                }
            }

            return returnValue;
        }
        public static bool RestoreApp(string dateString, string appName)
        {
            var returnValue = false;

            if (DataServiceSettings.Applications != null && DataServiceSettings.Applications.ContainsKey(appName))
            {
                try
                {
                    returnValue = GetBackupFileContents(dateString, appName, out string fileContents);
                    DataServiceSettings.Applications[appName] = JsonConvert.DeserializeObject<Application>(fileContents);
                }
                catch (Exception ex)
                {
                    returnValue = false;
                    WDSLogs.WriteServiceDBLog(LogLevels.Error, $"Restoring backup: {ex.Message}");
                }
            }

            return returnValue;
        }
        public static bool RestoreConnections(string dateString)
        {
            var returnValue = false;

            try
            {
                returnValue = GetBackupFileContents(dateString, "connections", out string fileContents);
                lock (_lockObj)
                {
                    DataServiceSettings.Connections = JsonConvert.DeserializeObject<Connections>(fileContents);
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
                WDSLogs.WriteServiceDBLog(LogLevels.Error, $"Restoring connections: {ex.Message}");
            }

            return returnValue;
        }
        public static bool RestoreAPIKeys(string dateString)
        {
            var returnValue = false;

            try
            {
                returnValue = GetBackupFileContents(dateString, "apikeys", out string fileContents);
                lock (_lockObj)
                {
                    DataServiceSettings.ApiKeys = new ApiKeys(JsonConvert.DeserializeObject<ApiKeysModel>(fileContents));
                }
            }
            catch (Exception ex)
            {
                WDSLogs.WriteServiceDBLog(LogLevels.Error, $"Restoring APIKeys: {ex.Message}");
                returnValue = false;
            }

            return returnValue;
        }
        private static bool GetBackupFileContents(string dateString, string fileName, out string fileContents)
        {
            var returnValue = false;
            fileContents = string.Empty; 

            string directory = $"{DataServiceSettings.Connections.ConnectionByName[WDSCache.WD_CACHE_ID].GetConnection()}\\settingsBackup\\{dateString}";
            if (Directory.Exists(directory))
            {
                // Connections
                var filePath = Path.Combine(directory, $"{directory}\\{fileName}.{WDSCache.WD_CACHE_FILE_EXTENSION}");
                if (File.Exists(filePath))
                {
                    fileContents = File.ReadAllText(filePath);
                    if (fileContents.Length > 0)
                    {
                        fileContents = AesOperation.DecryptString(WDSCache.CACHED_DATA_ENCRYPT_KEY, fileContents);                        
                    }

                    returnValue = true;
                }
            }

            return returnValue; 
        }
        private static bool ContainsAppRecordset(string app, string recordset)
        {
            return !app.IsNullOrEmpty() && !recordset.IsNullOrEmpty() &&
                Applications != null && Applications.ContainsKey(app) && 
                Applications[app].Recordsets != null && Applications[app].Recordsets.ContainsKey(recordset);
        }
        public static Recordset Rs(string app, string recordset)
        {
            return ContainsAppRecordset(app, recordset) ? Applications[app].Recordsets[recordset] : null; 
        }

        public static Recordset Rs(string call)
        {
            var sep = call.IndexOf('.');
            if (sep < 1)
                return null; 

            var appName = call.Substring(0, sep);
            var rsName = call.Substring(sep + 1);

            return Rs(appName, rsName); 
        }
    }
}
