using System.ComponentModel.DataAnnotations.Schema;

namespace WDS.Models.JperStat
{
    public class JperstatModel
    {
        [Column("RowNbr")]
        public int RowNbr { get; set; }
        [Column("Country")]
        public string Country { get; set; }
        [Column("MIL")]
        public int? MIL { get; set; }
        [Column("Mil chg")]
        public int? MilChg { get; set; }
        [Column("CIV")]
        public int? CIV { get; set; }
        [Column("CIV Chg")]
        public int? CivChg { get; set; }

    }
}
