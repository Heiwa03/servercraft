using System.Collections.Generic;
using System.Threading.Tasks;
using servercraft.Models;
using servercraft.Models.ViewModels;

namespace eUseControl.BusinessLogic.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ServerViewModel>> GetAllServersAsync();
        Task<ServerViewModel> GetServerByIdAsync(string id);
        Task<IEnumerable<ServerViewModel>> SearchServersAsync(string query);
        Task<bool> CreateServerAsync(Server server);
    }
} 