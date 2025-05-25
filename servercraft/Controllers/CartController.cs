// Controllers/CartController.cs
using System;
using System.Collections.Generic;
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
    public class CartController : Controller, ICartController
    {
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(ICartService cartService, IUnitOfWork unitOfWork)
        {
            _cartService = cartService;
            _unitOfWork = unitOfWork;
        }

        // GET: /Cart/
        public async Task<ActionResult> Index()
        {
            var cartId = GetCartId();
            var viewModel = await _cartService.GetCartAsync(cartId);
            return View(viewModel);
        }

        // GET: /Cart/Checkout
        public async Task<ActionResult> Checkout()
        {
            var cartId = GetCartId();
            var viewModel = await _cartService.GetCartAsync(cartId);

            if (!viewModel.Items.Any())
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // POST: /Cart/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProcessPayment(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            var cartId = GetCartId();
            var viewModel = await _cartService.GetCartAsync(cartId);

            if (!viewModel.Items.Any())
            {
                return RedirectToAction("Index");
            }

            // Process payment based on selected method
            switch (model.PaymentMethod)
            {
                case PaymentMethod.BankAccount:
                    // In a real application, you would integrate with a payment processor
                    // For now, we'll just simulate a successful payment
                    break;

                case PaymentMethod.GooglePay:
                    // Redirect to Google Pay
                    return Redirect("https://pay.google.com/checkout");

                case PaymentMethod.PayPal:
                    // Redirect to PayPal
                    return Redirect("https://www.paypal.com/checkout");

                default:
                    ModelState.AddModelError("", "Invalid payment method selected.");
                    return View("Checkout", model);
            }

            // Clear the cart after successful payment
            foreach (var item in viewModel.Items)
            {
                await _cartService.RemoveFromCartAsync(cartId, item.ServerId);
            }

            // Clear the cart ID from the session
            Session.Remove("CartId");
            Session.Remove("CartCount");

            // Redirect to a success page
            return RedirectToAction("PaymentSuccess");
        }

        // GET: /Cart/PaymentSuccess
        public ActionResult PaymentSuccess()
        {
            return View();
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public async Task<ActionResult> AddToCart(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var cartId = GetCartId();
            var success = await _cartService.AddToCartAsync(cartId, id);

            if (success)
            {
                var cartCount = await _cartService.GetCartCountAsync(cartId);
                Session["CartCount"] = cartCount;
                return Json(new { success = true, cartCount = cartCount });
            }

            return Json(new { success = false });
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public async Task<ActionResult> UpdateQuantity(string id, int quantity)
        {
            var cartId = GetCartId();
            var success = await _cartService.UpdateQuantityAsync(cartId, id, quantity);

            if (success)
            {
                var cartCount = await _cartService.GetCartCountAsync(cartId);
                Session["CartCount"] = cartCount;
                return RedirectToAction("Index");
            }

            return HttpNotFound();
        }

        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(string id)
        {
            var cartId = GetCartId();
            var success = await _cartService.RemoveFromCartAsync(cartId, id);

            if (success)
            {
                var cartCount = await _cartService.GetCartCountAsync(cartId);
                var total = await _cartService.GetCartTotalAsync(cartId);
                Session["CartCount"] = cartCount;

                return Json(new
                {
                    success = true,
                    cartCount,
                    total
                });
            }

            return Json(new { success = false });
        }

        // POST: /Cart/UpdateQuantityAjax
        [HttpPost]
        public async Task<ActionResult> UpdateQuantityAjax(string id, int quantity)
        {
            var cartId = GetCartId();
            var success = await _cartService.UpdateQuantityAsync(cartId, id, quantity);

            if (success)
            {
                var cartCount = await _cartService.GetCartCountAsync(cartId);
                var total = await _cartService.GetCartTotalAsync(cartId);
                Session["CartCount"] = cartCount;

                return Json(new
                {
                    success = true,
                    cartCount,
                    total
                });
            }

            return Json(new { success = false });
        }

        // POST: /Cart/UpdateCartBatch
        [HttpPost]
        public async Task<ActionResult> UpdateCartBatch(List<CartUpdateItem> updates)
        {
            var cartId = GetCartId();
            var success = await _cartService.UpdateCartBatchAsync(cartId, updates);

            if (success)
            {
                var cartCount = await _cartService.GetCartCountAsync(cartId);
                var total = await _cartService.GetCartTotalAsync(cartId);
                Session["CartCount"] = cartCount;

                return Json(new
                {
                    success = true,
                    cartCount,
                    total
                });
            }

            return Json(new { success = false });
        }

        // Helper method to get or create cart ID
        private string GetCartId()
        {
            if (Session["CartId"] == null)
            {
                // Create a new cart ID
                Guid tempCartId = Guid.NewGuid();
                Session["CartId"] = tempCartId.ToString();
            }

            return Session["CartId"].ToString();
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

    public class CartUpdateItem
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}