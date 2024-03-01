using System.Collections.Generic;
using DataService.Core.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using DataService.Models.Settings;
using System.Linq;
using System;
using DataService.Core.Logging;
using WDS.Data;
using static WDSClient.Models.WDSEnums;

namespace DataService.Core.Settings
{
    public class Parameter
    {
        ParameterModel _dataContext;

        public Parameter(ParameterModel dataContext)
        {
            _dataContext = dataContext;
        }

        public ParameterModel Model
        {
            get
            {
                return _dataContext;
            }
        }

        public void AddToDB(string appName, string recordsetName)
        {
            try
            {
                var sql = SQLUpdate(appName, recordsetName, out Dictionary<string, string> parameters);
                WDSWriteOperations.UpdateDB(sql, parameters);

            }
            catch (Exception ex) 
            {
                WDSLogs.WriteServiceDBLog(LogLevels.Warning, $"Unable to add the Parameter ({appName}/{recordsetName}/{Model.Name}) to the database. {ex.Message}");
            }
        }

        public string SQLUpdate(string appName, string recordsetName, out Dictionary<string, string> parameters)
        {
            parameters = new Dictionary<string, string>();
            parameters.Add("parameterName", _dataContext.Name);
            parameters.Add("callParameterName", _dataContext.RecordsetParameter);
            parameters.Add("required", _dataContext.Required.ToString());
            parameters.Add("regExMask", _dataContext.RegularExpression);
            parameters.Add("defaultValue", _dataContext.DefaultValue);
            parameters.Add("replaceWith", _dataContext.ReplaceWith);
            parameters.Add("isActive", "1");
            parameters.Add("appName", appName);
            parameters.Add("recordsetName", recordsetName);

            return $"EXEC [wds].[add_parameter]";
        }

        public static List<ParameterModel> GetParametersFromDB(string appName, string recordsetName, SqlConnection sqlConnection)
        {
            var parameters = new Dictionary<string, string>()
                {
                    { "appName", appName },
                    { "recordsetName", recordsetName }
                };
            var newParameters = SQLHelpers
                .GetData<ParameterModel>("[wds].[get_parameters]", sqlConnection, CommandType.StoredProcedure, parameters);
            
            return newParameters;
        }
    }
}
