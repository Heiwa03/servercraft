// Models/ServerMarketInitializer.cs
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using servercraft.Models.Domain;
using servercraft.Models.Repositories;

namespace servercraft.Models
{
    public class ServerMarketInitializer : DropCreateDatabaseIfModelChanges<ServerMarketContext>
    {
        protected override void Seed(ServerMarketContext context)
        {
            var unitOfWork = new UnitOfWork(context);
            SeedAsync(unitOfWork).Wait();
        }

        private async Task SeedAsync(IUnitOfWork unitOfWork)
        {
            var servers = new List<Server>
            {
                new Server
                {
                    Id = "server-1",
                    Name = "PowerEdge R740 Rack Server",
                    Description = "High-performance 2U rack server designed for complex workloads",
                    Price = 4999,
                    OldPrice = 5499,
                    ImageUrl = "/Content/images/servers/server1.jpg",
                    Badge = "Best Seller",
                    InStock = true,
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Gold 6248R 3.0GHz, 24C/48T",
                        Memory = "64GB DDR4-2933MHz ECC Registered Memory",
                        Storage = "2TB NVMe SSD + 4TB 7.2K RPM SATA HDD",
                        Network = "Dual 10GbE SFP+ + Dual 1GbE RJ45",
                        Power = "Dual 750W Redundant Power Supplies",
                        FormFactor = "2U Rack"
                    }
                },
                new Server
                {
                    Id = "server-2",
                    Name = "ProLiant DL380 Gen10 Server",
                    Description = "Versatile rack server with industry-leading performance",
                    Price = 5499,
                    ImageUrl = "/Content/images/servers/server2.jpg",
                    InStock = true,
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Silver 4210 2.2GHz, 10C/20T",
                        Memory = "32GB DDR4-2933MHz ECC Registered Memory",
                        Storage = "1.2TB 10K SAS Storage",
                        Network = "Quad-port Gigabit Ethernet",
                        Power = "Dual 500W Redundant Power Supplies",
                        FormFactor = "2U Rack"
                    }
                }
            };

            foreach (var server in servers)
            {
                await unitOfWork.Servers.AddAsync(server);
            }

            // Add specifications for each server
            var specs1 = new List<ServerSpecification>
            {
                new ServerSpecification { ServerId = "server-1", Description = "Intel Xeon Gold 6248R 3.0GHz, 24C/48T" },
                new ServerSpecification { ServerId = "server-1", Description = "64GB DDR4 ECC Memory" },
                new ServerSpecification { ServerId = "server-1", Description = "2TB SSD + 4TB HDD Storage" },
                new ServerSpecification { ServerId = "server-1", Description = "Dual 10GbE Network Adapters" },
                new ServerSpecification { ServerId = "server-1", Description = "Redundant Power Supplies" }
            };

            var specs2 = new List<ServerSpecification>
            {
                new ServerSpecification { ServerId = "server-2", Description = "Intel Xeon Silver 4210 2.2GHz, 10C/20T" },
                new ServerSpecification { ServerId = "server-2", Description = "32GB DDR4 ECC Memory" },
                new ServerSpecification { ServerId = "server-2", Description = "1.2TB SAS Storage" },
                new ServerSpecification { ServerId = "server-2", Description = "Quad-port Gigabit Ethernet" },
                new ServerSpecification { ServerId = "server-2", Description = "iLO Advanced Management" }
            };

            foreach (var spec in specs1)
            {
                await unitOfWork.ServerSpecifications.AddAsync(spec);
            }

            foreach (var spec in specs2)
            {
                await unitOfWork.ServerSpecifications.AddAsync(spec);
            }

            await unitOfWork.CompleteAsync();
        }
    }
}