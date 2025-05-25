using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using servercraft.Models.ViewModels;

namespace eUseControl.BusinessLogic.Interfaces.Controllers
{
    public interface ICartController
    {
        Task<ActionResult> Index();
        Task<ActionResult> Checkout();
        Task<ActionResult> ProcessPayment(CheckoutViewModel model);
        ActionResult PaymentSuccess();
        Task<ActionResult> AddToCart(string id);
        Task<ActionResult> UpdateQuantity(string id, int quantity);
        Task<ActionResult> RemoveFromCart(string id);
        Task<ActionResult> UpdateQuantityAjax(string id, int quantity);
        Task<ActionResult> UpdateCartBatch(List<CartUpdateItem> updates);
    }
} 