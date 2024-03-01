using DbConnector.Core.Extensions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace WDS.Models.JperStat
{
    public class CCJ3CommandModel
    {
        public const string SpCCJ3GetCommandFeed = "dbo.get_CCJ3_Command_Feed";

        #region Map
        [Column("country")]
        [JsonPropertyName("Country")]
        public string Country { get; set; }

        [Column("loc")]
        [JsonPropertyName("loc")]
        public string Loc { get; set; }

        [Column("lat_lon")]
        [JsonPropertyName("lat_lon")]
        public string LatLon { get; set; }

        [Column("entry_date")]
        [JsonPropertyName("entry_date")]
        public DateTime EntryDate { get; set; }

        [Column("AFCENT")]
        [JsonPropertyName("AFCENT")]
        public int AFCENT { get; set; }

        [Column("ARCENT")]
        [JsonPropertyName("ARCENT")]
        public int ARCENT { get; set; }

        [Column("MARCENT")]
        [JsonPropertyName("MARCENT")]
        public int MARCENT { get; set; }

        [Column("NAVCENT")]
        [JsonPropertyName("NAVCENT")]
        public int NAVCENT { get; set; }

        [Column("SOCCENT")]
        [JsonPropertyName("SOCCENT")]
        public int SOCCENT { get; set; }

        [Column("DOMAIN FWD")]
        [JsonPropertyName("DOMAIN FWD")]
        public int DOMAINFwd { get; set; }

        [Column("USFOR-A FWD")]
        [JsonPropertyName("USFOR-A FWD")]
        public int UsforAFwd { get; set; }

        [Column("TASK FORCE 94-7.1")]
        [JsonPropertyName("TASK FORC 94-7.1")]
        public int TaskForce { get; set; }

        [Column("OSC-I")]
        [JsonPropertyName("OSC-I")]
        public int OSCI { get; set; }

        [Column("CJTF-OIR")]
        [JsonPropertyName("CJTF-OIR")]
        public int CjtfOir { get; set; }

        [Column("SCO")]
        [JsonPropertyName("SCO")]
        public int SCO { get; set; }

        [Column("ODRP")]
        [JsonPropertyName("ODRP")]
        public int ODRP { get; set; }

        [Column("DSCMO-A")]
        [JsonPropertyName("DSCMO-A")]
        public int DSCMOA { get; set; }

        [Column("JTF DRAGON")]
        [JsonPropertyName("JTF DRAGON")]
        public int JTFDRAGON { get; set; }

        [Column("COA")]
        [JsonPropertyName("COA")]
        public int COA { get; set; }

        [Column("US MIL")]
        [JsonPropertyName("US MIL")]
        public int USMIL { get; set; }

        [Column("US CIV")]
        [JsonPropertyName("US CIV")]
        public int UsCiv { get; set; }

        [Column("US TOTAL")]
        [JsonPropertyName("US TOTAL")]
        public int UsTotal { get; set; }

        [Column("BOG")]
        [JsonPropertyName("BOG")]
        public int BOG { get; set; }

        [Column("OTH")]
        [JsonPropertyName("OTH")]
        public int OTH { get; set; }

        [Column("RIP")]
        [JsonPropertyName("RIP")]
        public int RIP { get; set; }

        [Column("TCA")]
        [JsonPropertyName("TCA")]
        public int TCA { get; set; }

        [Column("TEF")]
        [JsonPropertyName("TEF")]
        public int TEF { get; set; }

        #endregion Map

    }
}
