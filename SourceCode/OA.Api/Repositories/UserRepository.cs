using Microsoft.EntityFrameworkCore;
using OA.Api.Interfaces;
using OA.Api.Requests;
using OA.Api.Responses;
using OA.Common.Helper;
using OA.Entities;
using System.Text;


namespace OA.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OAContext _context;

        public UserRepository(OAContext context)
        {
            _context = context;
        }

        private async Task<string?> CheckDuplicateAsync(string username, string email, string phoneNumber, int id)
        {
            var users = await _context.User.Where(u => (u.Username == username
                                                    || u.PhoneNumber == phoneNumber
                                                    || u.Email == email) && u.Id != id)
                                           .ToListAsync();

            if (!users.Any())
            {
                return null;
            }

            var errors = new StringBuilder();
            if (users.Any(u => u.Username == username))
            {
                errors.AppendLine("Username is duplicated");
            }

            if (users.Any(u => u.Email == email))
            {
                errors.AppendLine("Email is duplicated");
            }

            if (users.Any(u => u.PhoneNumber == phoneNumber))
            {
                errors.AppendLine("PhoneNumber is duplicated");
            }

            return errors.ToString();
        }

        public async Task<string?> CreateAsync(CreateUserRequest model)
        {
            var duplicateMessage = await CheckDuplicateAsync(model.Username, model.Email, model.PhoneNumber, model.Id);
            if (duplicateMessage != null)
            {
                return duplicateMessage;
            }

            var user = new User
            {
                Username = model.Username,
                Password = PasswordHelper.EncryptPassword(model.Password),
                Role = model.Role,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                IsActive = true,
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<AuthenticateResponse?> AuthenticateAsync(string username, string password)
        {
            byte[] arrayPassword = PasswordHelper.EncryptPassword(password);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username && u.Password == arrayPassword && u.IsActive);

            return user == null ? null : new AuthenticateResponse
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                FullName = user.FullName
            };
        }
        public async Task<UserResponse[]> GetAllAsync()
        {
            return await _context.User.Select(user => new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = user.IsActive,
            }).ToArrayAsync();
        }

        public async Task<string?> UpdateAsync(UpdateUserRequest model)
        {
            var duplicateMessage = await CheckDuplicateAsync(model.Username, model.Email, model.PhoneNumber, model.Id);
            if (duplicateMessage != null)
            {
                return duplicateMessage;
            }

            var entity = await _context.User.SingleAsync(ns => ns.Id == model.Id);
            {
                entity.Username = model.Username;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    entity.Password = PasswordHelper.EncryptPassword(model.Password);
                }
                entity.Role = model.Role;
                entity.Email = model.Email;
                entity.FullName = model.FullName;
                entity.PhoneNumber = model.PhoneNumber;
                entity.Address = model.Address;
                entity.IsActive = model.IsActive;
            };
            await _context.SaveChangesAsync();
            return null;
        }
        public async Task<UserResponse?> GetUserByUserIdAsync(int id)
        {
            var user = await _context.User.SingleAsync(u => u.Id == id);

            return user == null ? null : new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Password = PasswordHelper.DecryptPassword(user.Password),
                Role = user.Role,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                IsActive = true,
            };
        }

        public async Task<string?> ForgotPasswordAsync(string email)
        {
            var entity = await _context.User.FirstOrDefaultAsync(ns => ns.Email == email);
            if (entity == null || entity.Email == null)
            {
                return "Email is invalid";
            }
            var random = new Random();
            entity.Password = PasswordHelper.EncryptPassword(random.Next(100000, 999999).ToString());
            var subject = "Your Account Information";
            var body = "This is your username: " + entity.Username + "\nThis is your new password: " + PasswordHelper.DecryptPassword(entity.Password);
            await EmailHelper.SendMailAsync(entity.Email, body, subject);
            await _context.SaveChangesAsync();

            return null;
        }
    }
}
