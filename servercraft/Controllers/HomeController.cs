// Controllers/HomeController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Servercraft.Domain.Entities;
using Servercraft.Domain.Repositories;
using Servercraft.Data.Context;
using Servercraft.Model.ViewModels;

namespace servercraft.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var servers = await _unitOfWork.Servers.GetAllAsync();
            var viewModel = new HomeViewModel
            {
                FeaturedServers = servers.Take(6).Select(s => new ServerViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    Price = s.Price,
                    OldPrice = s.OldPrice,
                    ImageUrl = s.ImageUrl,
                    Badge = s.Badge,
                    InStock = s.InStock,
                    Category = s.Category
                }).ToList()
            };

            return View(viewModel);
        }

        public async Task<ActionResult> QuickView(string id)
        {
            var server = await _unitOfWork.Servers.GetByIdAsync(id);
            if (server == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ServerViewModel
            {
                Id = server.Id,
                Name = server.Name,
                Description = server.Description,
                Price = server.Price,
                OldPrice = server.OldPrice,
                ImageUrl = server.ImageUrl,
                Badge = server.Badge,
                InStock = server.InStock,
                Category = server.Category,
                Specifications = server.Specifications.Select(s => s.Description).ToList(),
                FullSpecs = server.FullSpecs
            };

            var viewModel = ServerViewModel.FromDomain(server);
            return PartialView("_QuickView", viewModel);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}