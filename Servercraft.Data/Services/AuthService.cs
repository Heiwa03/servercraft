using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Servercraft.Domain.Entities;
using Servercraft.Domain.Repositories;

namespace Servercraft.Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Username == username)).FirstOrDefault();
            if (user == null)
                return false;

            var hashedPassword = HashPassword(password);
            return user.PasswordHash == hashedPassword;
        }

        public async Task<bool> RegisterUserAsync(string username, string password, string email)
        {
            var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Username == username || u.Email == email)).FirstOrDefault();
            if (existingUser != null)
                return false;

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                return false;

            var hashedOldPassword = HashPassword(oldPassword);
            if (user.PasswordHash != hashedOldPassword)
                return false;

            user.PasswordHash = HashPassword(newPassword);
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
    }
} 