using System.Threading.Tasks;
using System.Web.Mvc;
using servercraft.Models;

namespace eUseControl.BusinessLogic.Interfaces.Controllers
{
    public interface IProductController
    {
        Task<ActionResult> Index();
        Task<ActionResult> Details(string id);
        Task<ActionResult> Search(string query);
        ActionResult Create();
        Task<ActionResult> Create(Server model);
    }
} 