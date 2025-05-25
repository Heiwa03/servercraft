using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUseControl.Domain.Models;
using eUseControl.Domain.Models.ViewModels;
using eUseControl.Domain.Interfaces.Services;
using servercraft.Models.Repositories;

namespace eUseControl.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ServerViewModel>> GetAllServersAsync()
        {
            var servers = await _unitOfWork.Servers.GetAllAsync();
            return servers.Select(ServerViewModel.FromDomain);
        }

        public async Task<ServerViewModel> GetServerByIdAsync(string id)
        {
            var server = await _unitOfWork.Servers.GetByIdAsync(id);
            return server != null ? ServerViewModel.FromDomain(server) : null;
        }

        public async Task<IEnumerable<ServerViewModel>> SearchServersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<ServerViewModel>();
            }

            var servers = await _unitOfWork.Servers.FindAsync(s =>
                s.Name.Contains(query) ||
                s.Description.Contains(query) ||
                s.Category.Contains(query));

            return servers.Select(ServerViewModel.FromDomain);
        }

        public async Task<bool> CreateServerAsync(Server server)
        {
            if (server == null)
            {
                return false;
            }

            server.Id = Guid.NewGuid().ToString();
            server.DateCreated = DateTime.UtcNow;

            _unitOfWork.Servers.Add(server);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
} 