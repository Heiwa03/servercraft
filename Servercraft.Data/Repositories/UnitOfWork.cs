using System;
using System.Threading.Tasks;
using Servercraft.Domain.Repositories;
using Servercraft.Domain.Entities;
using Servercraft.Data.Context;

namespace Servercraft.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ServerMarketContext _context;
        private bool _disposed;

        public UnitOfWork(ServerMarketContext context)
        {
            _context = context;
            Users = new Repository<User>(_context);
            Roles = new Repository<Role>(_context);
            UserRoles = new Repository<UserRole>(_context);
            Servers = new Repository<Server>(_context);
            CartItems = new Repository<CartItem>(_context);
            ServerFullSpecs = new Repository<ServerFullSpecs>(_context);
            ServerSpecifications = new Repository<ServerSpecification>(_context);
        }

        public IRepository<User> Users { get; }
        public IRepository<Role> Roles { get; }
        public IRepository<UserRole> UserRoles { get; }
        public IRepository<Server> Servers { get; }
        public IRepository<CartItem> CartItems { get; }
        public IRepository<ServerFullSpecs> ServerFullSpecs { get; }
        public IRepository<ServerSpecification> ServerSpecifications { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
} 