using System.Threading.Tasks;
using System.Linq;
using servercraft.Models.Repositories;
using eUseControl.Domain.Models.ViewModels;
using eUseControl.Domain.Interfaces.Services;

namespace eUseControl.BusinessLogic.Services
{
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServerViewModel> GetServerDetailsAsync(string id)
        {
            var server = await _unitOfWork.Servers.GetByIdAsync(id);
            return server != null ? ServerViewModel.FromDomain(server) : null;
        }

        public async Task<ServerViewModel[]> GetFeaturedServersAsync()
        {
            var servers = await _unitOfWork.Servers.GetAllAsync();
            return servers.Select(ServerViewModel.FromDomain).ToArray();
        }
    }
} 