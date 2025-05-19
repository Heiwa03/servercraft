// Controllers/CartController.cs
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using servercraft.Models;
using servercraft.Models.Domain;
using servercraft.Models.ViewModels;

namespace servercraft.Controllers
{
    public class CartController : Controller
    {
        private readonly ServerMarketContext db = new ServerMarketContext();

        // GET: /Cart/
        public ActionResult Index()
        {
            var cartId = GetCartId();
            var cartItems = db.CartItems
                .Include(c => c.Server)
                .Where(c => c.CartId == cartId)
                .ToList();

            var viewModel = new CartViewModel();

            foreach (var item in cartItems)
            {
                viewModel.Items.Add(new CartItemViewModel
                {
                    ServerId = item.ServerId,
                    Name = item.Server.Name,
                    ImageUrl = item.Server.ImageUrl,
                    Price = item.Server.Price,
                    Quantity = item.Quantity,
                    Total = item.Server.Price * item.Quantity
                });
            }

            viewModel.Subtotal = viewModel.Items.Sum(i => i.Total);
            viewModel.Shipping = 250; // Fixed shipping cost
            viewModel.Tax = Math.Round(viewModel.Subtotal * 0.08m, 2); // 8% tax
            viewModel.Total = viewModel.Subtotal + viewModel.Shipping + viewModel.Tax;

            return View(viewModel);
        }

        // GET: /Cart/Checkout
        public ActionResult Checkout()
        {
            var cartId = GetCartId();
            var cartItems = db.CartItems
                .Include(c => c.Server)
                .Where(c => c.CartId == cartId)
                .ToList();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index");
            }

            var viewModel = new CheckoutViewModel();

            foreach (var item in cartItems)
            {
                viewModel.Items.Add(new CartItemViewModel
                {
                    ServerId = item.ServerId,
                    Name = item.Server.Name,
                    ImageUrl = item.Server.ImageUrl,
                    Price = item.Server.Price,
                    Quantity = item.Quantity,
                    Total = item.Server.Price * item.Quantity
                });
            }

            viewModel.Subtotal = viewModel.Items.Sum(i => i.Total);
            viewModel.Shipping = 250; // Fixed shipping cost
            viewModel.Tax = Math.Round(viewModel.Subtotal * 0.08m, 2); // 8% tax
            viewModel.Total = viewModel.Subtotal + viewModel.Shipping + viewModel.Tax;

            return View(viewModel);
        }

        // POST: /Cart/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessPayment(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", model);
            }

            var cartId = GetCartId();
            var cartItems = db.CartItems
                .Include(c => c.Server)
                .Where(c => c.CartId == cartId)
                .ToList();

            if (!cartItems.Any())
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
            db.CartItems.RemoveRange(cartItems);
            db.SaveChanges();

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
        public ActionResult AddToCart(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var cartId = GetCartId();

            // Check if the item is already in the cart
            var cartItem = db.CartItems
                .FirstOrDefault(c => c.CartId == cartId && c.ServerId == id);

            if (cartItem == null)
            {
                // Create a new cart item
                cartItem = new CartItem
                {
                    CartId = cartId,
                    ServerId = id,
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                db.CartItems.Add(cartItem);
            }
            else
            {
                // Increment quantity
                cartItem.Quantity++;
            }

            db.SaveChanges();

            // Get the total number of items in the cart
            var cartCount = db.CartItems
                .Where(c => c.CartId == cartId)
                .Sum(c => c.Quantity);

            Session["CartCount"] = cartCount;

            return Json(new { success = true, cartCount = cartCount });
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public ActionResult UpdateQuantity(string id, int quantity)
        {
            var cartId = GetCartId();

            var cartItem = db.CartItems
                .FirstOrDefault(c => c.CartId == cartId && c.ServerId == id);

            if (cartItem == null)
            {
                return HttpNotFound();
            }

            if (quantity > 0)
            {
                cartItem.Quantity = quantity;
            }
            else
            {
                db.CartItems.Remove(cartItem);
            }

            db.SaveChanges();

            // Get the total number of items in the cart
            var cartCount = db.CartItems
                .Where(c => c.CartId == cartId)
                .Sum(c => c.Quantity);

            Session["CartCount"] = cartCount;

            return RedirectToAction("Index");
        }

        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public ActionResult RemoveFromCart(string id)
        {
            var cartId = GetCartId();
            var cartItem = db.CartItems.FirstOrDefault(c => c.CartId == cartId && c.ServerId == id);
            if (cartItem != null)
            {
                db.CartItems.Remove(cartItem);
                db.SaveChanges();
            }
            // Recalculate totals
            var cartItems = db.CartItems.Include(c => c.Server).Where(c => c.CartId == cartId).ToList();
            var subtotal = cartItems.Sum(i => i.Server.Price * i.Quantity);
            var shipping = 250m;
            var tax = Math.Round(subtotal * 0.08m, 2);
            var total = subtotal + shipping + tax;
            var cartCount = cartItems.Sum(i => i.Quantity);
            Session["CartCount"] = cartCount;
            return Json(new {
                success = true,
                cartCount,
                subtotal,
                shipping,
                tax,
                total
            });
        }

        // POST: /Cart/UpdateQuantityAjax
        [HttpPost]
        public ActionResult UpdateQuantityAjax(string id, int quantity)
        {
            var cartId = GetCartId();
            var cartItem = db.CartItems.Include(c => c.Server).FirstOrDefault(c => c.CartId == cartId && c.ServerId == id);
            if (cartItem == null)
            {
                return Json(new { success = false });
            }
            if (quantity > 0)
            {
                cartItem.Quantity = quantity;
            }
            else
            {
                db.CartItems.Remove(cartItem);
            }
            db.SaveChanges();
            // Recalculate totals
            var cartItems = db.CartItems.Include(c => c.Server).Where(c => c.CartId == cartId).ToList();
            var subtotal = cartItems.Sum(i => i.Server.Price * i.Quantity);
            var shipping = 250m;
            var tax = Math.Round(subtotal * 0.08m, 2);
            var total = subtotal + shipping + tax;
            var cartCount = cartItems.Sum(i => i.Quantity);
            Session["CartCount"] = cartCount;
            var itemTotal = cartItem != null && quantity > 0 ? cartItem.Server.Price * cartItem.Quantity : 0;
            return Json(new {
                success = true,
                cartCount,
                subtotal,
                shipping,
                tax,
                total,
                itemTotal
            });
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}