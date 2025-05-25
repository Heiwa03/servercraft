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
using servercraft.Models.Domain;
using eUseControl.BusinessLogic.Services;
using eUseControl.BusinessLogic.Interfaces.Services;
using eUseControl.BusinessLogic.Interfaces.Controllers;

namespace servercraft.Controllers
{
    public class ProductController : Controller, IProductController
    {
        private readonly IProductService _productService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IProductService productService, IUnitOfWork unitOfWork)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
        }

        // GET: /Product/
        public async Task<ActionResult> Index()
        {
            try
            {
                var servers = await _productService.GetAllServersAsync();
                return View(servers.ToList());
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
                var server = await _productService.GetServerByIdAsync(id);
                if (server == null)
                {
                    return HttpNotFound();
                }

                // Get related products
                var relatedServers = await _unitOfWork.Servers.FindAsync(s => s.Id != id);
                ViewBag.RelatedServers = relatedServers
                    .Take(4)
                    .Select(ServerViewModel.FromDomain)
                    .ToList();

                return View(server);
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
            var servers = await _productService.SearchServersAsync(query);
            ViewBag.Query = query;
            return View("SearchResults", servers.ToList());
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
                var success = await _productService.CreateServerAsync(model);
                if (success)
                {
                    return RedirectToAction("Index");
                }
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