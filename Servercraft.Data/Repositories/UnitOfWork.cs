using System.Threading.Tasks;
using servercraft.Models.Domain;

namespace Servercraft.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ServerMarketContext _context;
        private Repository<Server> _servers;
        private Repository<ServerSpecification> _serverSpecifications;
        private Repository<ServerFullSpecs> _serverFullSpecs;
        private Repository<CartItem> _cartItems;
        private Repository<User> _users;
        private Repository<Role> _roles;

        public UnitOfWork(ServerMarketContext context)
        {
            _context = context;
        }

        public IRepository<Server> Servers
        {
            get
            {
                if (_servers == null)
                {
                    _servers = new Repository<Server>(_context);
                }
                return _servers;
            }
        }

        public IRepository<ServerSpecification> ServerSpecifications
        {
            get
            {
                if (_serverSpecifications == null)
                {
                    _serverSpecifications = new Repository<ServerSpecification>(_context);
                }
                return _serverSpecifications;
            }
        }

        public IRepository<ServerFullSpecs> ServerFullSpecs
        {
            get
            {
                if (_serverFullSpecs == null)
                {
                    _serverFullSpecs = new Repository<ServerFullSpecs>(_context);
                }
                return _serverFullSpecs;
            }
        }

        public IRepository<CartItem> CartItems
        {
            get
            {
                if (_cartItems == null)
                {
                    _cartItems = new Repository<CartItem>(_context);
                }
                return _cartItems;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new Repository<User>(_context);
                }
                return _users;
            }
        }

        public IRepository<Role> Roles
        {
            get
            {
                if (_roles == null)
                {
                    _roles = new Repository<Role>(_context);
                }
                return _roles;
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
} 