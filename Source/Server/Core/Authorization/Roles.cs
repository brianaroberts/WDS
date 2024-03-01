using DataService.Core.Data;
using DataService.Core.Logging;
using DataService.Core.Settings;
using DataService.Models.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WDS.Authentication;
using WDSClient.Models;
using static WDSClient.Models.WDSEnums;

namespace WDS.Authorization
{
    public static class Roles
    {   
        public static string WDS_Admin { get; } = "WDS_Admin";
        public static string WDS_DataConsumer { get; } = "WDS_DataConsumer";
        public static string WDS_DataWriter { get; } = "WDS_DataWriter";

        public static List<string> GetRolesFromAPIKey(int apiKeyId)
        {
            var returnValue = new List<string>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DataServiceSettings.WDSConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlCommand command = new SqlCommand($"EXEC [wds].[get_roles_frm_APIKey] '{apiKeyId}'", sqlConnection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable objTable = new DataTable();
                        adapter.Fill(objTable);

                        foreach (DataRow row in objTable.Rows)
                        {
                            returnValue.Add(row[0].ToString());                       
                        }

                    }
                    sqlConnection.Close();
                }
            } catch (Exception ex) 
            {
                WDSLogs.WriteServiceDBLog(LogLevels.Error, ex.Message);
            }

            return returnValue; 
        }

        public static bool WriteRolesToDB(string apiKeyName, List<string> roles)
        {
            var returnValue = false; 
            var parameters = new Dictionary<string, string>();
            parameters.Add("apiKeyName", apiKeyName);
            parameters.Add("roles", string.Join(",", roles));
                        
            string sqlStatement = $"EXEC [wds].[add_APIKey_application_role]";
            WDSWriteOperations.UpdateDB(sqlStatement, parameters);

            return returnValue; 
        }
    }
}
