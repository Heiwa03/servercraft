using System.Threading.Tasks;
using eUseControl.Domain.Models.ViewModels;

namespace eUseControl.Domain.Interfaces.Services
{
    public interface IHomeService
    {
        Task<ServerViewModel> GetServerDetailsAsync(string id);
        Task<ServerViewModel[]> GetFeaturedServersAsync();
    }
} 