using System.Threading.Tasks;

namespace Servercraft.Data.Services
{
    public interface IAuthService
    {
        Task<bool> ValidateUserAsync(string username, string password);
        Task<bool> RegisterUserAsync(string username, string password, string email);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> IsInRoleAsync(int userId, string roleName);
    }
} 