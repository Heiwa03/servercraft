using System.Collections.Generic;
using System.Linq;
using servercraft.Models;

namespace eUseControl.BusinessLogic
{
    public class ServerService
    {
        private readonly BusinessLogicDbContext _context;

        public ServerService(BusinessLogicDbContext context)
        {
            _context = context;
        }

        public List<Server> GetAllServers()
        {
            return _context.Servers.ToList();
        }

        public Server GetServerById(int id)
        {
            return _context.Servers.Find(id);
        }

        public void AddServer(Server server)
        {
            _context.Servers.Add(server);
            _context.SaveChanges();
        }

        public void UpdateServer(Server server)
        {
            _context.Entry(server).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteServer(int id)
        {
            var server = _context.Servers.Find(id);
            if (server != null)
            {
                _context.Servers.Remove(server);
                _context.SaveChanges();
            }
        }
    }
} 