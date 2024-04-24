using System.ComponentModel.DataAnnotations;

namespace OA.Api.Responses
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public byte Role { get; set; }

        public string? FullName { get; set; }

        public string? Token { get; set; }
    }
}
