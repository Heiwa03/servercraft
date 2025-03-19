// Models/ViewModels/ServerViewModel.cs
using System.Collections.Generic;
using servercraft.Models.Domain;

namespace servercraft.Models.ViewModels
{
    public class ServerViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string ImageUrl { get; set; }
        public string Badge { get; set; }
        public bool InStock { get; set; }
        public List<string> Specs { get; set; }
        public ServerFullSpecsViewModel FullSpecs { get; set; }

        public ServerViewModel()
        {
            Specs = new List<string>();
        }

        public static ServerViewModel FromDomain(Server server)
        {
            var viewModel = new ServerViewModel
            {
                Id = server.Id,
                Name = server.Name,
                Description = server.Description,
                Price = server.Price,
                OldPrice = server.OldPrice,
                ImageUrl = server.ImageUrl,
                Badge = server.Badge,
                InStock = server.InStock
            };

            foreach (var spec in server.Specifications)
            {
                viewModel.Specs.Add(spec.Description);
            }

            if (server.FullSpecs != null)
            {
                viewModel.FullSpecs = new ServerFullSpecsViewModel
                {
                    Processor = server.FullSpecs.Processor,
                    Memory = server.FullSpecs.Memory,
                    Storage = server.FullSpecs.Storage,
                    Network = server.FullSpecs.Network,
                    Power = server.FullSpecs.Power,
                    FormFactor = server.FullSpecs.FormFactor
                };
            }

            return viewModel;
        }
    }

    public class ServerFullSpecsViewModel
    {
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Storage { get; set; }
        public string Network { get; set; }
        public string Power { get; set; }
        public string FormFactor { get; set; }
    }
}