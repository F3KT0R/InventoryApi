using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApi.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")] // Maps to the lowercase 'id' column
        public int Id { get; set; }

        [Column("created_at")] // Maps to the 'created_at' column
        public DateTime CreatedAt { get; set; }

        [Column("name")] // Maps to the lowercase 'name' column
        public required string Name { get; set; }

        [Column("email")] // Maps to the lowercase 'email' column
        public required string Email { get; set; }

        [Column("status")] // Maps to the lowercase 'status' column
        public required string Status { get; set; }
    }
}
