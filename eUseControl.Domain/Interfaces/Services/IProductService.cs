using System.Collections.Generic;
using System.Threading.Tasks;
using eUseControl.Domain.Models;
using eUseControl.Domain.Models.ViewModels;

namespace eUseControl.Domain.Interfaces.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ServerViewModel>> GetAllServersAsync();
        Task<ServerViewModel> GetServerByIdAsync(string id);
        Task<IEnumerable<ServerViewModel>> SearchServersAsync(string query);
        Task<bool> CreateServerAsync(Server server);
    }
} 