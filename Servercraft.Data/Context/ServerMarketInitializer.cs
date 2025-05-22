using System.Data.Entity;
using Servercraft.Domain.Entities;

namespace Servercraft.Data.Context
{
    public class ServerMarketInitializer : DropCreateDatabaseIfModelChanges<ServerMarketContext>
    {
        protected override void Seed(ServerMarketContext context)
        {
            // Add default roles
            var adminRole = new Role { Id = "1", Name = "Administrator" };
            var userRole = new Role { Id = "2", Name = "User" };

            context.Roles.Add(adminRole);
            context.Roles.Add(userRole);

            // Add sample servers
            var server1 = new Server
            {
                Id = "1",
                Name = "Basic Server",
                Description = "A basic server for small businesses",
                Price = 99.99m,
                ImageUrl = "/Content/Images/server1.jpg"
            };

            var server2 = new Server
            {
                Id = "2",
                Name = "Enterprise Server",
                Description = "High-performance server for enterprise use",
                Price = 299.99m,
                ImageUrl = "/Content/Images/server2.jpg"
            };

            context.Servers.Add(server1);
            context.Servers.Add(server2);

            base.Seed(context);
        }
    }
} 