// Controllers/ProductController.cs
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using Servercraft.Domain.Entities;
using Servercraft.Model.ViewModels;
using Servercraft.Domain.Repositories;
using Servercraft.Data.Context;

namespace servercraft.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        [Authorize]
        public ActionResult Create()
        {
            // Only allow admin
            if (User.Identity.Name != "admin")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Server model)
        {
            if (User.Identity.Name != "admin")
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid().ToString();
                await _unitOfWork.Servers.AddAsync(model);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View(model);
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