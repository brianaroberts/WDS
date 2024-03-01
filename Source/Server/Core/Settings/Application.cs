using DataService.Core.Data;
using DataService.Core.Logging;
using DataService.Models.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;
using WDS.Core.Extensions;
using WDS.Data;
using WDS.Settings;
using WDSClient.Models;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Settings
{
    public class Application
    {
        private ApplicationModel _dataContext;

        public string Name
        {
            get
            {
                return _dataContext.Name;
            }
        }
        public string DataOwner
        {
            get { return _dataContext.DataOwner; }
        }
        public string Description
        {
            get { return _dataContext.Description; }
        }
        private object _lockObj = new object();

        public ApplicationModel Model { get { return _dataContext; } }

        public Application(ApplicationModel dataContext, bool addToDB = false)
        {
            if (dataContext == null) return; 
            _dataContext = dataContext;

            if (_dataContext.Recordsets != null)
            {
                foreach (var rs in dataContext.Recordsets.Where(o => !o.ConnectionName.IsNullOrEmpty() && !DataServiceSettings.Connections.ConnectionByName.ContainsKey(o.ConnectionName)))
                {
                    WDSLogs.WriteServiceDBLog(LogLevels.Information, $"Adding connection to database {rs.ConnectionName}"); 
                    var model = new ConnectionModel();
                    model.Name = rs.ConnectionName;
                    model.ConnectionType = ConnectionTypes.Database;
                    DataServiceSettings.Connections.UpdateConnectionToDB(model);
                }
            }

            if (addToDB)
            {
                AddToDB();
            }
        }

        public static Dictionary<string, Application> GetApplicationsFromDB(SqlConnection sqlConnection)
        {
            var returnValue = new Dictionary<string, Application>(StringComparer.OrdinalIgnoreCase);
            var parameters = new Dictionary<string, string>()
                {
                    { "hostName", DataServiceSettings.Connections.HostName }
                };
            var newApplications = SQLHelpers
                .GetData<ApplicationModel>("[wds].[get_applications]", sqlConnection, CommandType.StoredProcedure, parameters);

            foreach (var application in newApplications)
            {
                application.Recordsets = Settings.Recordset.GetRecordsetsFromDB(application.Name, sqlConnection);
                application.SourcedFrom = SourcedFromTypes.Database; 
                
                if (!returnValue.ContainsKey(application.Name))
                    returnValue.Add(application.Name, new Application(application, false));
            }

            return returnValue;
        }

        public Dictionary<string, Recordset> Recordsets
        {
            get
            {
                return _dataContext.Recordsets.Where(o=>!o.Name.IsNullOrEmpty()).ToDictionary(o => o.Name, o => new Recordset(o), StringComparer.OrdinalIgnoreCase);
            }
        }

        private void AddToDB()
        {
            try
            {
                WDSWriteOperations.UpdateDB(SQLUpdate);
            }
            catch (Exception ex) 
            {
                WDSLogs.WriteServiceDBLog(LogLevels.Warning, $"Unable to add Application ({Name}) to the database. {ex.Message}"); 
            }

            foreach (var recordset in _dataContext.Recordsets)
            {
                new Recordset(recordset, true);
            }
        }

        private string SQLUpdate(out Dictionary<string, string> parameters)
        {
            parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            parameters.Add("hostName", DataServiceSettings.Connections.HostName);
            parameters.Add("name", _dataContext.Name);
            parameters.Add("description", _dataContext.Description);
            parameters.Add("dataOwner", _dataContext.DataOwner);

            return $"EXEC [wds].[add_Application]";
        }

        public bool AddRecordset(RecordsetModel recordsetToAdd, bool updateDB = false)
        {
            var returnValue = false;

            if (recordsetToAdd == null || Recordsets.ContainsKey(recordsetToAdd.Name)
                || recordsetToAdd.Name.IsNullOrEmpty()
                ) return returnValue;

            _dataContext.Recordsets.Add(recordsetToAdd);

            if (updateDB)
            {
                Dictionary<string, string> parameters;
                var rs = new Recordset(recordsetToAdd);
                rs.AddToDB();

                returnValue = true;
            }

            return returnValue;
        }
    }
}
