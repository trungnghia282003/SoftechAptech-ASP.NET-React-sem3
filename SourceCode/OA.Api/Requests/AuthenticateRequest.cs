using System.ComponentModel.DataAnnotations;

namespace OA.Api.Requests
{
    public class AuthenticateRequest
    {
        [Required]
        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Password { get; set; }
    }
}
