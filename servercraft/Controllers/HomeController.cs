// Controllers/HomeController.cs
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using servercraft.Models;
using servercraft.Models.ViewModels;
using eUseControl.BusinessLogic.Services;
using servercraft.Models.Repositories;
using eUseControl.Domain.Interfaces.Controllers;
using eUseControl.Domain.Interfaces.Services;

namespace servercraft.Controllers
{
    public class HomeController : Controller, IHomeController
    {
        private readonly IHomeService _homeService;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IHomeService homeService, IUnitOfWork unitOfWork)
        {
            _homeService = homeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ActionResult> Index()
        {
            var featuredServers = await _homeService.GetFeaturedServersAsync();
            return View(featuredServers);
        }

        public async Task<ActionResult> QuickView(string id)
        {
            var server = await _homeService.GetServerDetailsAsync(id);
            if (server == null)
            {
                return HttpNotFound();
            }

            return PartialView("_QuickView", server);
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