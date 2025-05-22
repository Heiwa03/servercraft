using System.Collections.Generic;

namespace Servercraft.Model.ViewModels
{
    public class HomeViewModel
    {
        public List<ServerViewModel> FeaturedServers { get; set; }

        public HomeViewModel()
        {
            FeaturedServers = new List<ServerViewModel>();
        }
    }
} 