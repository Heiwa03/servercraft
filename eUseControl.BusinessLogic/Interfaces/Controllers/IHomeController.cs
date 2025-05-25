using System.Threading.Tasks;
using System.Web.Mvc;

namespace eUseControl.BusinessLogic.Interfaces.Controllers
{
    public interface IHomeController
    {
        Task<ActionResult> Index();
        Task<ActionResult> QuickView(string id);
        ActionResult About();
        ActionResult Contact();
    }
} 