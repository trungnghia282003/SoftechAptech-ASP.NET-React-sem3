using OA.Api.Requests;
using OA.Api.Responses;

namespace OA.Api.Interfaces
{
    public interface IUserRepository
    {
        Task<string?> CreateAsync(CreateUserRequest model);

        Task<AuthenticateResponse?> AuthenticateAsync(string username, string password);

        Task<UserResponse[]> GetAllAsync();

        Task<string?> UpdateAsync(UpdateUserRequest model);

        Task<UserResponse?> GetUserByUserIdAsync(int id);

        Task<string?> ForgotPasswordAsync(string email);
    }
}
