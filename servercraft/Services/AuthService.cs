using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using servercraft.Models;
using servercraft.Models.Domain;
using servercraft.Models.Repositories;

namespace servercraft.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == username && u.IsActive);
            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            user.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.CompleteAsync();

            return user;
        }

        public async Task<User> RegisterAsync(string username, string password, string email, string firstName, string lastName)
        {
            if (await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == username) != null)
                throw new Exception("Username already exists");

            if (await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == email) != null)
                throw new Exception("Email already exists");

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            // Add user role by default
            var userRole = await _unitOfWork.Roles.SingleOrDefaultAsync(r => r.Name == "User");
            if (userRole == null)
            {
                userRole = new Role { Name = "User" };
                await _unitOfWork.Roles.AddAsync(userRole);
            }

            user.UserRoles.Add(new UserRole { Role = userRole });
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return user;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return false;

            if (!VerifyPassword(oldPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> IsInRoleAsync(int userId, string roleName)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return false;

            return user.UserRoles.Any(ur => ur.Role.Name == roleName);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
} 