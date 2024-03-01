using System.ComponentModel.DataAnnotations.Schema;

namespace WDS.Models.ARC
{
    public class PermRecord
    {
        [Column("rank_id")]
        public string? Rank { get; set; }
        [Column("last_name")]
        public string? LastName { get; set; }
        [Column("first_name")]
        public string? FirstName { get; set; }
        [Column("mi")]
        public string? MI { get; set; }
        [Column("clearance")]
        public string? Clearance { get; set; }
    }
}