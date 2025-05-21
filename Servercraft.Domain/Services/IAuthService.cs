using System.Threading.Tasks;
using Servercraft.Domain.Entities;

namespace Servercraft.Domain.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> RegisterAsync(string username, string password, string email, string firstName, string lastName);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> IsInRoleAsync(int userId, string roleName);
    }
} 