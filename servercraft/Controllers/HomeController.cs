// Controllers/HomeController.cs
using System.Linq;
using System.Web.Mvc;
using Servercraft.Domain.Entities;
using Servercraft.Model.ViewModels;
using Servercraft.Data.Context;

namespace Servercraft.Web.Controllers
{
    public class HomeController : Controller
    {
        private ServerMarketContext db = new ServerMarketContext();

        public ActionResult Index()
        {
            var featuredServers = db.Servers
                .ToList()
                .Select(ServerViewModel.FromDomain)
                .ToList();

            return View(featuredServers);
        }

        public ActionResult QuickView(string id)
        {
            var server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }

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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}