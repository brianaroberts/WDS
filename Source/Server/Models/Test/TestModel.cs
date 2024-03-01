using System;

namespace DataService.Models.Test
{
    public class TestModel
    {
        public TestModel(WDSServerResponse response)
        {
            Successful = !response.ContainsError;
            ExecutionTime = response.ExecutionDuration;
            NumRows = response.GetData().Rows.Count;
            ErrorMessage = response.ErrorMessage;
        }
        public bool Successful { get; set; }
        public int NumRows { get; set; }
        public string ExecutionTime { get; set; }
        public string ErrorMessage { get; set; }
    }
}
