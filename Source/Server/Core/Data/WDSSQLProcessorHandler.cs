using System.Data;
using WDS.Models;
using DataService.Models.Settings;
using System;
using WDS.Core.Extensions;
using DataService.Models;
using WDSClient.Models;
using DataService.Core.Logging;
using System.Collections.Generic;
using static WDSClient.Models.WDSEnums;

namespace WDS.Data
{
    public class WDSSQLProcessorHandler : IWDSCallProcessor
    {
        public List<CallTypes> SupportedProcessorType { get => new List<CallTypes>() { CallTypes.SQL, CallTypes.StoredProc }; }

        public WDSServerResponse MakeCall(WDSServerRequest request)
        {
            WDSServerResponse response = new WDSServerResponse(request);            
            response.StartTimer();

            try
            {
                response.StartRecordsetRetrivalTimer();
                DataTable objCallDataTable = SQLHelpers.GetDataTable(request.Statement, request.GetConnectionName(), SQLHelpers.GetCommandType(request.CallType), request.CallParameters);

                response.ResponseDataFormatType = request.ResponseDataFormatType;                
                response.ReturnValue = ReturnValueFormatter(objCallDataTable, response.ResponseDataFormatType);
                response.StopRecordsetRetrivalTimer();
            }
            catch (Exception e)
            {
                response.ErrorMessage = $"({request.Application}.{request.Call}) Error in FromSQLServer: {e.Message}"; 
                WDSLogs.WriteServiceDBLog(LogLevels.Error, response.ErrorMessage);
            }

            response.StopTimer();
            return response;
        }

       

        private string ReturnValueFormatter(DataTable dataTable, DataFormatTypes type)
        {
            string returnValue;

            switch (type)
            {
                case DataFormatTypes.Email:
                case DataFormatTypes.Json:
                    returnValue = dataTable.ToJson();
                    break;
                case DataFormatTypes.Xml:
                    returnValue = dataTable.ToXml();
                    break;
                case DataFormatTypes.CSV:
                case DataFormatTypes.NotSet:
                default:
                    returnValue = dataTable.ToCsv();
                    break;
            }
            return returnValue;
        }

        
    }
}
