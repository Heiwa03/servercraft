// Controllers/ProductController.cs
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using servercraft.Models;
using servercraft.Models.ViewModels;

namespace servercraft.Controllers
{
    public class ProductController : Controller
    {
        private ServerMarketContext db = new ServerMarketContext();

        // GET: /Product/
        public ActionResult Index()
        {
            var servers = db.Servers
                .ToList()
                .Select(ServerViewModel.FromDomain)
                .ToList();

            return View(servers);
        }

        // GET: /Product/Details/5
        [OutputCache(Duration = 60, VaryByParam = "id")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var server = db.Servers
                .Include(s => s.Specifications)
                .Include(s => s.FullSpecs)
                .FirstOrDefault(s => s.Id == id);

            if (server == null)
            {
                return HttpNotFound();
            }

            var viewModel = ServerViewModel.FromDomain(server);

            // Get related products
            var relatedServers = db.Servers
                .Where(s => s.Id != id)
                .Take(4)
                .ToList()
                .Select(ServerViewModel.FromDomain)
                .ToList();

            ViewBag.RelatedServers = relatedServers;

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}