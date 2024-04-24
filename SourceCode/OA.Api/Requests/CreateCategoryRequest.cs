using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OA.Api.Requests
{
    public class CreateCategoryRequest
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        [Column(TypeName = "ntext")]
        public string? Description { get; set; }

        public byte Role { get; set; }

        public bool IsActive { get; set; }
    }
}
