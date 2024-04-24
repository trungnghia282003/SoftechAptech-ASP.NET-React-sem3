using System.ComponentModel.DataAnnotations;

namespace OA.Api.Requests
{
    public class CreateUserRequest
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }

        public byte Role { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string? FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PhoneNumber { get; set; }

        [Required]
        [MaxLength(300)]
        public string? Address { get; set; }

        public bool IsActive { get; set; }
    }
}
