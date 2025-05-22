using System;
using System.Threading.Tasks;
using Servercraft.Domain.Entities;

namespace Servercraft.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Server> Servers { get; }
        IRepository<ServerSpecification> ServerSpecifications { get; }
        IRepository<ServerFullSpecs> ServerFullSpecs { get; }
        IRepository<CartItem> CartItems { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<UserRole> UserRoles { get; }
        Task<int> CompleteAsync();
    }
} 