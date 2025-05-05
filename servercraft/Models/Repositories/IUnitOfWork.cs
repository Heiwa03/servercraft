using System;
using System.Threading.Tasks;
using servercraft.Models.Domain;

namespace servercraft.Models.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Domain.Server> Servers { get; }
        IRepository<Domain.ServerSpecification> ServerSpecifications { get; }
        IRepository<Domain.ServerFullSpecs> ServerFullSpecs { get; }
        IRepository<Domain.CartItem> CartItems { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        Task<int> CompleteAsync();
    }
} 