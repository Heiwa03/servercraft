using System.Threading.Tasks;
using servercraft.Models.ViewModels;

namespace eUseControl.BusinessLogic.Interfaces.Services
{
    public interface IHomeService
    {
        Task<ServerViewModel> GetServerDetailsAsync(string id);
        Task<ServerViewModel[]> GetFeaturedServersAsync();
    }
} 