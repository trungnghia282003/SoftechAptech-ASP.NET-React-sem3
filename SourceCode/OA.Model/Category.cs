using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OA.Entities
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
