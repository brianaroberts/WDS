using Microsoft.Data.SqlClient;
using System.Data;
using System;
using WDS.Core.Extensions;
using DataService.Core.Settings;
using System.Collections.Generic;
using System.Linq;
using DataService.Core.Data;
using static WDSClient.Models.WDSEnums;
using DataService.Core.Logging;

namespace WDS.Data
{
    public static class SQLHelpers 
    {
        public static DataTable GetDataTable(string statement, string connectionName, CommandType cmdType, Dictionary<string, string> parameters)
        {
            DataTable returnDatatable = new DataTable();
            string connectionString = connectionName.Length > 25 ? connectionName : DataServiceSettings.Connections.ConnectionByName[connectionName].GetConnection();

            if (connectionString.IsNullOrEmpty()) throw new Exception($"Connection string for ID:{connectionName} is empty");

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    returnDatatable = GetDataTable(statement, sqlConnection, cmdType, parameters);
                    sqlConnection.Close();
                }
            } catch (Exception ex) 
            {
                WDSLogs.WriteServiceDBLog(LogLevels.Error, ex.Message);
            }

            return returnDatatable; 
        }

        public static DataTable GetDataTable(string statement, SqlConnection sqlConnection, CommandType cmdType, Dictionary<string, string> parameters)
        {
            DataTable returnDatatable = new DataTable();

            var sqlStatement = statement;

            using (SqlCommand command = GetSqlCommand(sqlStatement, sqlConnection, cmdType, parameters))
            {
                using (SqlDataReader sqlDataReader = command.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        returnDatatable.Load(sqlDataReader);
                    }
                }
            }

            return returnDatatable;
        }

        private static SqlCommand GetSqlCommand(string sqlStatement, SqlConnection connection, CommandType commandType, Dictionary<string,string> callParameters)
        {
            var sqlCommand = new SqlCommand();

            sqlCommand.CommandType = commandType;
            sqlCommand.Connection = connection;
            sqlCommand.CommandText = sqlStatement;

            foreach (string key in callParameters.Keys)
            {
                if (callParameters[key].ToLower() == "null")
                {
                    sqlCommand.Parameters.AddWithValue($"@{key}", DBNull.Value);
                }
                else
                    sqlCommand.Parameters.AddWithValue($"@{key}", callParameters[key]);
            }

            return sqlCommand; 
        }

        public static List<TEntity> GetData<TEntity>(string command, string connectionName, CommandType cmdType, Dictionary<string, string> parameters) where TEntity : class, new()
        {
            var returnValue = new List<TEntity>();

            var dt = GetDataTable(command, connectionName, cmdType, parameters);

            returnValue = DataNamesMapper<TEntity>.Map(dt).ToList();


            return returnValue;
        }

        public static List<TEntity> GetData<TEntity>(string command, SqlConnection sqlConnection, CommandType cmdType, Dictionary<string, string> parameters) where TEntity : class, new()
        {
            var returnValue = new List<TEntity>();

            var dt = GetDataTable(command, sqlConnection, cmdType, parameters);

            returnValue = DataNamesMapper<TEntity>.Map(dt).ToList();


            return returnValue;
        }

        public static CommandType GetCommandType(CallTypes callType)
        {
            var returnValue = CommandType.Text;

            switch (callType)
            {
                case CallTypes.NotSet:
                case CallTypes.StoredProc:
                    returnValue = CommandType.StoredProcedure;
                    break;
                case CallTypes.SQL: // Already Set                    
                default:
                    break;
            }

            return returnValue;
        }
    }
}
