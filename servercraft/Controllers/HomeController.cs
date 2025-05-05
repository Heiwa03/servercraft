// Controllers/HomeController.cs
using System.Linq;
using System.Web.Mvc;
using servercraft.Models;
using servercraft.Models.ViewModels;

namespace servercraft.Controllers
{
    public class HomeController : Controller
    {
        private ServerMarketContext db = new ServerMarketContext();

        public ActionResult Index()
        {
            var featuredServers = db.Servers
                .Take(6)
                .ToList()
                .Select(ServerViewModel.FromDomain)
                .ToList();

            return View(featuredServers);
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