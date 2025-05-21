// Models/ServerMarketInitializer.cs
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Servercraft.Domain.Entities;
using Servercraft.Data.Repositories;

namespace Servercraft.Data
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
                // Enterprise Servers
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
                    Category = "Enterprise",
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
                    Category = "Enterprise",
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Silver 4210 2.2GHz, 10C/20T",
                        Memory = "32GB DDR4-2933MHz ECC Registered Memory",
                        Storage = "1.2TB 10K SAS Storage",
                        Network = "Quad-port Gigabit Ethernet",
                        Power = "Dual 500W Redundant Power Supplies",
                        FormFactor = "2U Rack"
                    }
                },
                new Server
                {
                    Id = "server-5",
                    Name = "ThinkSystem SR650",
                    Description = "High-density enterprise server for mission-critical workloads",
                    Price = 6999,
                    OldPrice = 7499,
                    ImageUrl = "/Content/images/servers/server5.jpg",
                    Badge = "New",
                    InStock = true,
                    Category = "Enterprise",
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Platinum 8380 2.3GHz, 40C/80T",
                        Memory = "128GB DDR4-3200MHz ECC Registered Memory",
                        Storage = "4TB NVMe SSD + 8TB 7.2K RPM SATA HDD",
                        Network = "Dual 25GbE SFP28 + Dual 10GbE RJ45",
                        Power = "Dual 1600W Redundant Power Supplies",
                        FormFactor = "2U Rack"
                    }
                },

                // Cloud Servers
                new Server
                {
                    Id = "server-3",
                    Name = "Cloud Server Pro",
                    Description = "High-performance cloud server for scalable applications",
                    Price = 2999,
                    ImageUrl = "/Content/images/servers/server3.jpg",
                    InStock = true,
                    Category = "Cloud",
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "AMD EPYC 7302P 3.0GHz, 16C/32T",
                        Memory = "32GB DDR4-3200MHz ECC Memory",
                        Storage = "1TB NVMe SSD",
                        Network = "Dual 25GbE SFP28",
                        Power = "Single 800W Power Supply",
                        FormFactor = "1U Rack"
                    }
                },
                new Server
                {
                    Id = "server-6",
                    Name = "CloudFlex X2",
                    Description = "Flexible cloud server with high-density compute",
                    Price = 3999,
                    ImageUrl = "/Content/images/servers/server6.jpg",
                    InStock = true,
                    Category = "Cloud",
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "AMD EPYC 7763 2.45GHz, 64C/128T",
                        Memory = "64GB DDR4-3200MHz ECC Memory",
                        Storage = "2TB NVMe SSD",
                        Network = "Dual 100GbE QSFP28",
                        Power = "Dual 1200W Redundant Power Supplies",
                        FormFactor = "2U Rack"
                    }
                },

                // Storage Servers
                new Server
                {
                    Id = "server-4",
                    Name = "Storage Server X",
                    Description = "High-capacity storage server for data-intensive workloads",
                    Price = 3999,
                    ImageUrl = "/Content/images/servers/server4.jpg",
                    InStock = true,
                    Category = "Storage",
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Silver 4214 2.2GHz, 12C/24T",
                        Memory = "64GB DDR4-2933MHz ECC Memory",
                        Storage = "8TB 7.2K RPM SATA HDD",
                        Network = "Dual 10GbE SFP+",
                        Power = "Dual 550W Redundant Power Supplies",
                        FormFactor = "2U Rack"
                    }
                },
                new Server
                {
                    Id = "server-7",
                    Name = "StorageMax Pro",
                    Description = "High-density storage server with advanced data protection",
                    Price = 5999,
                    OldPrice = 6499,
                    ImageUrl = "/Content/images/servers/server7.jpg",
                    Badge = "Popular",
                    InStock = true,
                    Category = "Storage",
                    FullSpecs = new ServerFullSpecs
                    {
                        Processor = "Intel Xeon Gold 6330 2.0GHz, 28C/56T",
                        Memory = "128GB DDR4-3200MHz ECC Memory",
                        Storage = "12x 16TB 7.2K RPM SATA HDD",
                        Network = "Dual 25GbE SFP28",
                        Power = "Dual 800W Redundant Power Supplies",
                        FormFactor = "4U Rack"
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

            var specs3 = new List<ServerSpecification>
            {
                new ServerSpecification { ServerId = "server-3", Description = "AMD EPYC 7302P 3.0GHz, 16C/32T" },
                new ServerSpecification { ServerId = "server-3", Description = "32GB DDR4 ECC Memory" },
                new ServerSpecification { ServerId = "server-3", Description = "1TB NVMe SSD Storage" },
                new ServerSpecification { ServerId = "server-3", Description = "Dual 25GbE Network" },
                new ServerSpecification { ServerId = "server-3", Description = "Cloud-Optimized Design" }
            };

            var specs4 = new List<ServerSpecification>
            {
                new ServerSpecification { ServerId = "server-4", Description = "Intel Xeon Silver 4214 2.2GHz, 12C/24T" },
                new ServerSpecification { ServerId = "server-4", Description = "64GB DDR4 ECC Memory" },
                new ServerSpecification { ServerId = "server-4", Description = "8TB HDD Storage" },
                new ServerSpecification { ServerId = "server-4", Description = "Dual 10GbE Network" },
                new ServerSpecification { ServerId = "server-4", Description = "High-Capacity Storage" }
            };

            var specs5 = new List<ServerSpecification>
            {
                new ServerSpecification { ServerId = "server-5", Description = "Intel Xeon Platinum 8380 2.3GHz, 40C/80T" },
                new ServerSpecification { ServerId = "server-5", Description = "128GB DDR4 ECC Memory" },
                new ServerSpecification { ServerId = "server-5", Description = "4TB NVMe + 8TB HDD Storage" },
                new ServerSpecification { ServerId = "server-5", Description = "Dual 25GbE + Dual 10GbE Network" },
                new ServerSpecification { ServerId = "server-5", Description = "High-Density Compute" }
            };

            var specs6 = new List<ServerSpecification>
            {
                new ServerSpecification { ServerId = "server-6", Description = "AMD EPYC 7763 2.45GHz, 64C/128T" },
                new ServerSpecification { ServerId = "server-6", Description = "64GB DDR4 ECC Memory" },
                new ServerSpecification { ServerId = "server-6", Description = "2TB NVMe SSD Storage" },
                new ServerSpecification { ServerId = "server-6", Description = "Dual 100GbE Network" },
                new ServerSpecification { ServerId = "server-6", Description = "Cloud-Optimized Design" }
            };

            var specs7 = new List<ServerSpecification>
            {
                new ServerSpecification { ServerId = "server-7", Description = "Intel Xeon Gold 6330 2.0GHz, 28C/56T" },
                new ServerSpecification { ServerId = "server-7", Description = "128GB DDR4 ECC Memory" },
                new ServerSpecification { ServerId = "server-7", Description = "192TB HDD Storage" },
                new ServerSpecification { ServerId = "server-7", Description = "Dual 25GbE Network" },
                new ServerSpecification { ServerId = "server-7", Description = "High-Density Storage" }
            };

            foreach (var spec in specs1) await unitOfWork.ServerSpecifications.AddAsync(spec);
            foreach (var spec in specs2) await unitOfWork.ServerSpecifications.AddAsync(spec);
            foreach (var spec in specs3) await unitOfWork.ServerSpecifications.AddAsync(spec);
            foreach (var spec in specs4) await unitOfWork.ServerSpecifications.AddAsync(spec);
            foreach (var spec in specs5) await unitOfWork.ServerSpecifications.AddAsync(spec);
            foreach (var spec in specs6) await unitOfWork.ServerSpecifications.AddAsync(spec);
            foreach (var spec in specs7) await unitOfWork.ServerSpecifications.AddAsync(spec);

            await unitOfWork.CompleteAsync();
        }
    }
}