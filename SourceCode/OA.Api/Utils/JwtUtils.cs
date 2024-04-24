using Microsoft.IdentityModel.Tokens;
using OA.Api.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QLTC.Application.Utils
{
    public static class JwtUtils
    {
        public static string GenerateJwtToken(AuthenticateResponse response)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("897166D289BB923C8BAF59ED918C4");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", response.Id.ToString()),
                    new Claim("Username", response.Username!),
                    new Claim("Role", response.Role.ToString()),
                    new Claim("FullName", response.FullName!),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static AuthenticateResponse? ValidateJwtToken(string? token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("897166D289BB923C8BAF59ED918C4");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken?.Claims?.ToArray();
                if (claims?.Length > 0)
                {
                    return new AuthenticateResponse
                    {
                        Id = int.Parse(claims.First(x => x.Type == "Id").Value),
                        Username = claims.First(x => x.Type == "Username").Value,
                        Role = byte.Parse(claims.First(x => x.Type == "Role").Value),
                        FullName = claims.First(x => x.Type == "FullName").Value
                    };
                }

                return null;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
