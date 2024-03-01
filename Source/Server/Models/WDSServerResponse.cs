using DataService.Models.Settings;
using System;
using System.Diagnostics;
using WDS.Models;

namespace DataService.Models
{
    public class WDSServerResponse : WDSResponseContract
    {
        private Stopwatch _executionTimer = new Stopwatch();
        private Stopwatch _sqlExecutionTimer = new Stopwatch();
        private string _executionDate;

        #region Properties
        public string ExecutionDate
        {
            get
            {
                if (string.IsNullOrEmpty(_executionDate))
                    _executionDate = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
                return _executionDate;
            }
            set
            {
                _executionDate = value;
            }
        }
        public string ExecutionDuration
        {
            get { return _executionTimer.Elapsed.ToString(); }
        }
        public string SQLExecutionDuration
        {
            get { return _sqlExecutionTimer.Elapsed.ToString(); }
        }
        public WDSServerRequest Request { get; set; }
        #endregion Properties

        #region constructors
        public WDSServerResponse(WDSServerRequest request)
        {
            Request = request;
            RequestedDataDetailType = request.RequestedDataDetailType;
        }

        public WDSServerResponse()
        {
        }
        #endregion constructors

        public void StartTimer()
        {
            _executionTimer.Start();
        }

        public void StopTimer()
        {
            _executionTimer.Stop();
        }

        public void StartRecordsetRetrivalTimer()
        {
            _sqlExecutionTimer.Start();
        }

        public void StopRecordsetRetrivalTimer()
        {
            _sqlExecutionTimer.Stop();
        }

    }
}
