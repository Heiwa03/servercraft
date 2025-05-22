using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Servercraft.Domain.Entities;
using Servercraft.Model.ViewModels;
using Servercraft.Domain.Repositories;
using Servercraft.Data.Services;
using Servercraft.Data.Context;
using System.Security.Claims;

namespace servercraft.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
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

            var result = await _authService.ValidateUserAsync(model.Username, model.Password);
            if (result)
            {
                var user = (await _unitOfWork.Users.FindAsync(u => u.Username == model.Username)).FirstOrDefault();
                if (user != null)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }, DefaultAuthenticationTypes.ApplicationCookie);
                    var authManager = HttpContext.GetOwinContext().Authentication;
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = model.RememberMe }, identity);

                    return RedirectToLocal(returnUrl);
                }
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
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
            if (ModelState.IsValid)
            {
                var result = await _authService.RegisterUserAsync(model.Username, model.Password, model.Email);
                if (result)
                {
                    return RedirectToAction("Login", new { message = "Registration successful. Please log in." });
                }
                else
                {
                    ModelState.AddModelError("", "Username or email already exists.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _unitOfWork.Users.FindAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await _authService.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await _authService.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _unitOfWork.Users.FindAsync(u => u.Email == model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _authService.ResetPasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
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
            var user = (await _unitOfWork.Users.FindAsync(u => u.Username == username)).FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            try
            {
                var success = await _authService.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
                if (success)
                {
                    TempData["Success"] = "Password changed successfully!";
                    return RedirectToAction("Profile", "Account");
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

        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var userId = User.Identity.Name;
            var users = await _unitOfWork.Users.FindAsync(u => u.Username == userId);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize]
        public async Task<ActionResult> AdminDashboard()
        {
            var username = User.Identity.Name;
            var user = (await _unitOfWork.Users.FindAsync(u => u.Username == username)).FirstOrDefault();
            
            if (user == null || !await _authService.IsInRoleAsync(user.Id, "Administrator"))
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
            // Create administrator account if it doesn't exist
            if (filterContext.ActionDescriptor.ActionName == "Login")
            {
                Task.Run(async () =>
                {
                    var adminRole = (await _unitOfWork.Roles.FindAsync(r => r.Name == "Administrator")).FirstOrDefault();
                    if (adminRole == null)
                    {
                        adminRole = new Role { Name = "Administrator" };
                        await _unitOfWork.Roles.AddAsync(adminRole);
                        await _unitOfWork.CompleteAsync();
                    }

                    var adminUser = (await _unitOfWork.Users.FindAsync(u => u.Username == "admin")).FirstOrDefault();
                    if (adminUser == null)
                    {
                        await _authService.RegisterUserAsync(
                            "admin",
                            "admin",
                            "admin@servercraft.com"
                        );

                        adminUser = (await _unitOfWork.Users.FindAsync(u => u.Username == "admin")).FirstOrDefault();
                        if (adminUser != null)
                        {
                            adminUser.UserRoles.Add(new UserRole { Role = adminRole });
                            await _unitOfWork.CompleteAsync();
                        }
                    }
                    else
                    {
                        // Ensure admin role is assigned to existing admin user
                        if (!adminUser.UserRoles.Any(ur => ur.Role.Name == "Administrator"))
                        {
                            adminUser.UserRoles.Add(new UserRole { Role = adminRole });
                            await _unitOfWork.CompleteAsync();
                        }
                    }
                }).Wait();
            }
        }

        protected new void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
} 