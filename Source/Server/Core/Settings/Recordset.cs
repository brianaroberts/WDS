using DataService.Core.Data;
using DataService.Core.Logging;
using DataService.Models.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using WDS.Core.Extensions;
using WDS.Data;
using WDS.Models;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Settings
{
    public class Recordset
    {
        private RecordsetModel _dataContext;

        public RecordsetModel Model
        {
            get
            {
                return _dataContext;
            }
        }
        public Dictionary<string, Parameter> Parameters
        {
            get
            {
                if (_dataContext!= null && _dataContext.Parameters != null && _dataContext.Parameters.Count > 0)
                    return _dataContext.Parameters.ToDictionary(o => o.Name, o => new Parameter(o), StringComparer.OrdinalIgnoreCase);
                else
                    return new Dictionary<string, Parameter>();
            }
        }

        #region Constructors
        public Recordset(RecordsetModel model, bool addToDB = false)
        {
            _dataContext = model;
            if (addToDB)
            {
                try
                {
                    AddToDB();
                }
                catch (Exception ex)
                {
                    WDSLogs.WriteServiceDBLog(LogLevels.Warning, $"Unable to add the Recordsets ({Model.Application}/{Model.Name}) to the database. {ex.Message}"); 
                }
            }
        }

        public Recordset()
        {
            _dataContext = new RecordsetModel();
        }
        #endregion Constructors

        public bool AreParametersValid(Dictionary<string, string> parametersToUpdate)
        {
            if (_dataContext.Parameters == null || _dataContext.Parameters.Count == 0) return true;
            if (parametersToUpdate == null) return !_dataContext.Parameters.Any(o => o.Required);

            // Missing = No Key or Null Value
            bool missingRequired = _dataContext.Parameters.Any(o => o.Required == true && (!parametersToUpdate.ContainsKey(o.Name) || parametersToUpdate[o.Name].IsNullOrEmpty()));
            // Only RegEx parameters with values
            bool badFormating = _dataContext.Parameters.Where(p => parametersToUpdate.ContainsKey(p.Name) && !parametersToUpdate[p.Name].IsNullOrEmpty()).Any(o =>
            {
                return !string.IsNullOrEmpty(o.RegularExpression) && !Regex.Match(parametersToUpdate[o.Name], o.RegularExpression).Success;
            });

            return !missingRequired && !badFormating;
        }
        public void AddParameter(ParameterModel paramterToAdd, bool updateDB = false)
        {
            _dataContext.Parameters.Add(paramterToAdd);
            if (updateDB)
            {
                var param = new Parameter(paramterToAdd);
                var sql = param.SQLUpdate(_dataContext.Application, _dataContext.Name, out Dictionary<string, string> sqlparams);
                WDSWriteOperations.UpdateDB(sql, sqlparams);
            }
        }

        public void AddToDB()
        {
            if (Model.Statement.IsNullOrEmpty()) Model.Statement = Model.Name;
                
            WDSWriteOperations.UpdateDB(SQLUpdate);
            foreach (var param in Parameters.Values)
            {
                param.AddToDB(_dataContext.Application, _dataContext.Name);
            }
        }

        public string SQLUpdate(out Dictionary<string, string> parameters)
        {
            parameters = new Dictionary<string, string>();
            parameters.Add("hostName", DataServiceSettings.Connections.HostName);
            parameters.Add("appName", _dataContext.Application);
            parameters.Add("recordsetName", _dataContext.Name);
            parameters.Add("substituteName", _dataContext.Substitute);
            parameters.Add("connectionName", _dataContext.ConnectionName);
            parameters.Add("roleName", _dataContext.Role);
            parameters.Add("call", _dataContext.Statement);
            parameters.Add("callType",((int)_dataContext.CallType).ToString());
            parameters.Add("returnType", ((int)_dataContext.ReturnType).ToString());
            parameters.Add("returnTypeDetailsID", ((int)_dataContext.DataDetailType).ToString());
            parameters.Add("cache", (_dataContext.IsCached ? 1 : 0).ToString());
            parameters.Add("cacheTimeOut", _dataContext.CacheTimeOut.ToString());
            parameters.Add("endPointRequired", (_dataContext.EndPointRequired ? 1 : 0).ToString());
            parameters.Add("logCall", (_dataContext.LogCallInDB ? 1 : 0).ToString());

            return $"EXEC [wds].[add_Recordset]";
        }

        public static List<RecordsetModel> GetRecordsetsFromDB(string appName, SqlConnection sqlConnection)
        {
            var parameters = new Dictionary<string, string>()
                {
                    { "appName", appName }
                };
            var newRecordsets = SQLHelpers.GetData<RecordsetModel>($"[wds].[get_Recordsets]", sqlConnection, CommandType.StoredProcedure, parameters);

            foreach (var recordset in newRecordsets)
                recordset.Parameters = Parameter.GetParametersFromDB(appName, recordset.Name, sqlConnection);

            return newRecordsets;
        }
    }
}
