using DataService.Core.Settings;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using WDS.Core.Extensions;

namespace DataService.Core.Data
{
    public static class WDSWriteOperations
    {
        public delegate string SQLUpdateDelegate(out Dictionary<string, string> parameters); 


        public static void UpdateDB(string sql, Dictionary<string, string> parameters)
        {
            if (!sql.IsNullOrEmpty() && !DataServiceSettings.WDSConnectionString.IsNullOrEmpty())
            {
                using (SqlConnection sqlConnection = new SqlConnection(DataServiceSettings.WDSConnectionString))
                {
                    sqlConnection.Open();

                    using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure; 

                        foreach (var parameter in parameters)
                        {
                            if (parameter.Value == null)
                                command.Parameters.AddWithValue($"@{parameter.Key}", DBNull.Value);
                            else
                                command.Parameters.AddWithValue($"@{parameter.Key}", parameter.Value);
                        }
                        command.ExecuteNonQueryAsync();
                    }
                    sqlConnection.Close();
                }
            }
        }

        public static void UpdateDB(SQLUpdateDelegate del)
        {
            var sql = del(out Dictionary<string, string> sqlParameters);
            UpdateDB(sql, sqlParameters); 
        }
    }
}
