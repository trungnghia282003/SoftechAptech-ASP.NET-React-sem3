namespace OA.Api.Responses
{
    public class UserResponse
    {
        public int? Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public byte? Role { get; set; }

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public bool? IsActive { get; set; }
    }
}
