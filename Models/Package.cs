using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApi.Models
{
    [Table("Packages")]
    public class Package
    {
        [Key]
        [Column("id")] // Maps to the lowercase 'id' column
        public required string Id { get; set; }

        [Column("surname")] // Maps to the lowercase 'surname' column
        public required string Surname { get; set; }

        [Column("weight")] // Maps to the lowercase 'weight' column
        public double Weight { get; set; }

        [Column("arrival_date")] // Maps to the 'arrival_date' column
        public DateTime ArrivalDate { get; set; }

        [Column("status")] // Maps to the lowercase 'status' column
        public required string Status { get; set; }
    }
}
