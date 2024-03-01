using DataService.Core.Logging;
using DataService.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WDS.Core.Extensions;
using WDS.Data;
using WDS.Settings;
using WDSClient;
using static DataService.Core.Settings.DataServiceSettings;
using static WDS.Settings.ConnectionsModel;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Settings
{
    public class Connections
    {
        public string HostName { get { return System.Environment.GetEnvironmentVariable("COMPUTERNAME");  } }
        private ConnectionsModel _dataContext = new ConnectionsModel();
        public ConnectionsModel Model
        {
            get
            {
                return _dataContext;
            }
        }

        public Dictionary<string, ConnectionModel> ConnectionByName
        {
            get
            {
                return _dataContext.Connections.ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase);
            }
        }

        public void RefreshDatabaseConnections(string connectionString, bool replaceExisting = true)
        {
            if (connectionString.IsNullOrEmpty()) return; 
            var parameters = new Dictionary<string, string>()
                {
                    { "hostName", DataServiceSettings.Connections.HostName }
                };

            var oldConnections = _dataContext.Connections; 
            _dataContext.Connections = SQLHelpers
                .GetData<ConnectionModel>("[wds].[get_connections]", connectionString, CommandType.StoredProcedure, parameters);
            _dataContext.Connections.ForEach(cn => cn.SourcedFrom = SourcedFromTypes.Database); 
            foreach ( var connection in oldConnections.Where(o => ConnectionByName[o.Name].SourcedFrom == SourcedFromTypes.Database))
            {
                if (ConnectionByName.ContainsKey(connection.Name))
                    ConnectionByName[connection.Name] = connection; 
                else
                    _dataContext.Connections.Add(connection);
            }
        }

        public void SetupConnections(string path)
        {
            _dataContext = JsonConvert.DeserializeObject<ConnectionsModel>(File.ReadAllText($"{path}\\Connections.json"));
            _dataContext.Connections.ForEach(cn => cn.SourcedFrom = SourcedFromTypes.File);
            
            foreach (var connection in _dataContext.Connections.Where(o=>o.InsertIntoDB))
            {
                UpdateConnectionToDB(connection);
            }

            RefreshDatabaseConnections("WDS", false);
        }

        public int UpdateConnectionToDB(ConnectionModel connection)
        {
            if (connection == null || connection.Name.IsNullOrEmpty()) return 0;
            var parameters = new Dictionary<string, string>();
            parameters.Add("hostName", connection.HostName);
            parameters.Add("name", connection.Name);
            parameters.Add("connectionType", connection.Type);
            parameters.Add("production", connection.Production);
            parameters.Add("staging", connection.Staging);
            parameters.Add("development", connection.Development);
            parameters.Add("isActive", "1");

            int returnValue = 0;

            string connectionString = ConnectionByName["WDS"].GetConnection();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                
                WDSLogs.AddLog($"Opening Connection to '{connectionString}'", LogLevels.Information); 
                
                try
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand("[wds].[up_connection]", sqlConnection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue($"@{parameter.Key}", parameter.Value);
                        }
                        command.Parameters.Add("@idOutput", SqlDbType.Int).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        returnValue = (int)command.Parameters["@idOutput"].Value;
                    }
                } catch (Exception ex) 
                {
                    WDSLogs.AddLog($"Unable to open connection to '{sqlConnection}': {ex.Message}", LogLevels.Error);
                }
                
            }
            return returnValue;
        }

        public void SetRunningEnvironment(IWebHostEnvironment env)
        {
            var appSetting = SettingsReader.AppSetting("AppKeys", "AppMode");
            ConnectionEnvironment = (ConnectionEnvironmentTypes)Enum.Parse(typeof(ConnectionEnvironmentTypes), appSetting);

            WDSLogs.AddLog($"ConnectionEnvironment set to '{ConnectionEnvironment}'", LogLevels.Information);
        }
    }
}
