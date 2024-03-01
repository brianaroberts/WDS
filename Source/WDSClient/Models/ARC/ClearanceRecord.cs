using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WDS.Models.ARC
{

    public class ClearanceRecord
    {
        [Column("display_name")]
        public string? Name { get; set; }
        [Column("clearance")]
        public string? Clearance { get; set; }
        [Column("clearance_dt")]
        public DateTime? ClearanceDate { get; set; }
        [Column("invest_type")]
        public string? InvestigationType { get; set; }        
        [Column("invest_dt")]
        public DateTime? InvestigationDate { get; set; }
        [Column("service")]
        public string? Service { get; set; }
        [Column("office_symb")]
        public string? OfficeSymbol{ get; set; }
        [Column("birth_state")]
        public string? BirthPlace { get; set; }        
        [Column("birth_dt")]
        public DateTime? DateOfBirth { get; set; }
    }
}