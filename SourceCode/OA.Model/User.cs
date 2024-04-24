using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OA.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        public byte[]? Password { get; set; }

        public byte Role { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "varchar")]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(300)]
        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Rating>? BuyerRatings { get; set; }
        public ICollection<Rating>? SellerRatings { get; set; }
    }
}
