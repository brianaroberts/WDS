using System.ComponentModel.DataAnnotations.Schema;

namespace WDS.Models.ARC
{
    public class VisitorRecord
    {
        [Column("rank")]
        public string? Rank { get; set; }
        [Column("last_name")]
        public string? LastName { get; set; }
        [Column("first_name")]
        public string? FirstName { get; set; }
        [Column("mi")]
        public string? MI { get; set; }
        [Column("country")]
        public string? Country { get; set; }
        [Column("clearance")]
        public string? Clearance { get; set; }
        [Column("rec_exp_dt")]
        public DateTime? ExpDate { get; set; }
        [Column("authorized_locations")]
        public string? Authorized { get; set; }
        [Column("unit_location")]
        public string? UnitLocation { get; set; }

    }
}