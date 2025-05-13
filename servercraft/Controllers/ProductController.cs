// Controllers/ProductController.cs
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using servercraft.Models;
using servercraft.Models.Repositories;
using servercraft.Models.ViewModels;

namespace servercraft.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController()
        {
            _unitOfWork = new UnitOfWork(new ServerMarketContext());
        }

        // GET: /Product/
        public async Task<ActionResult> Index()
        {
            try
            {
                var servers = await _unitOfWork.Servers.GetAllAsync().ConfigureAwait(false);
                var viewModels = servers.Select(ServerViewModel.FromDomain).ToList();
                return View(viewModels);
            }
            catch
            {
                // Log the exception
                return View("Error");
            }
        }

        // GET: /Product/Details/5
        [OutputCache(Duration = 60, VaryByParam = "id")]
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var server = await _unitOfWork.Servers.GetByIdAsync(id).ConfigureAwait(false);
                if (server == null)
                {
                    return HttpNotFound();
                }

                var viewModel = ServerViewModel.FromDomain(server);

                // Get related products
                var relatedServers = await _unitOfWork.Servers.FindAsync(s => s.Id != id).ConfigureAwait(false);
                ViewBag.RelatedServers = relatedServers
                    .Take(4)
                    .Select(ServerViewModel.FromDomain)
                    .ToList();

                return View(viewModel);
            }
            catch
            {
                // Log the exception
                return View("Error");
            }
        }

        // GET: /Product/Search
        public async Task<ActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View("SearchResults", new List<ServerViewModel>());
            }

            query = query.Trim().ToLower();
            var servers = await _unitOfWork.Servers.FindAsync(s => s.Name.ToLower().IndexOf(query) >= 0);
            var viewModels = servers.Select(ServerViewModel.FromDomain).ToList();
            ViewBag.Query = query;
            return View("SearchResults", viewModels);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}