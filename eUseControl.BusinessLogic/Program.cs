using System;
using Microsoft.EntityFrameworkCore;
using servercraft.Models;

namespace eUseControl.BusinessLogic
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<BusinessLogicDbContext>();
            optionsBuilder.UseSqlServer("DefaultConnection");

            // Instantiate DbContext
            using (var context = new BusinessLogicDbContext(optionsBuilder.Options))
            {
                // Create an instance of ServerService
                var serverService = new ServerService(context);

                // Example: Get all servers
                var servers = serverService.GetAllServers();
                Console.WriteLine("All Servers:");
                foreach (var server in servers)
                {
                    Console.WriteLine($"Server ID: {server.Id}, Name: {server.Name}");
                }

                // Example: Get a server by ID
                var serverById = serverService.GetServerById(1);
                if (serverById != null)
                {
                    Console.WriteLine($"Server by ID: {serverById.Name}");
                }

                // Example: Add a new server
                var newServer = new Server { Name = "New Server" };
                serverService.AddServer(newServer);
                Console.WriteLine("New server added.");

                // Example: Update a server
                if (serverById != null)
                {
                    serverById.Name = "Updated Server";
                    serverService.UpdateServer(serverById);
                    Console.WriteLine("Server updated.");
                }

                // Example: Delete a server
                serverService.DeleteServer(1);
                Console.WriteLine("Server deleted.");
            }
        }
    }
} 