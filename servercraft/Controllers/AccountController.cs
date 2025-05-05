using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using servercraft.Models;
using servercraft.Models.Domain;
using servercraft.Models.Repositories;
using servercraft.Models.ViewModels;
using servercraft.Services;

namespace servercraft.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController()
        {
            _unitOfWork = new UnitOfWork(new ServerMarketContext());
            _authService = new AuthService(_unitOfWork);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _authService.AuthenticateAsync(model.Username, model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _authService.RegisterAsync(
                    model.Username,
                    model.Password,
                    model.Email,
                    model.FirstName,
                    model.LastName
                );

                FormsAuthentication.SetAuthCookie(model.Username, false);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var username = User.Identity.Name;
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return HttpNotFound();
            }

            try
            {
                var success = await _authService.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
                if (success)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Current password is incorrect.");
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while changing your password.");
                return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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