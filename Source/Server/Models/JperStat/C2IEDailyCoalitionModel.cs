using DbConnector.Core.Extensions;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace WDS.Models.JperStat
{
    public class C2IEDailyCoalitionModel
    {
        #region Map

        public string EntryDate { get; set; }
        public int CommandId { get; set; }
        public string CommandName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string CoalitionCountry { get; set; }
        public int CoalitionCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        #endregion Map
    }
}
