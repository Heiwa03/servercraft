using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using servercraft.Models;
using servercraft.Models.Domain;
using servercraft.Models.Repositories;
using servercraft.Models.ViewModels;
using servercraft.Services;
using System.Linq;

namespace servercraft.Controllers
{
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
        public ActionResult Login(string returnUrl, string username = null, string password = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            
            // Handle direct admin login
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var model = new LoginViewModel { Username = username, Password = password };
                return Login(model, returnUrl).Result;
            }
            
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
                    
                    // Check if user is administrator
                    if (await _authService.IsInRoleAsync(user.Id, "Administrator"))
                    {
                        return RedirectToAction("AdminDashboard", "Account");
                    }
                    
                    return RedirectToAction("Profile", "Account");
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
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == username);
            
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
                    var adminRole = await _unitOfWork.Roles.SingleOrDefaultAsync(r => r.Name == "Administrator");
                    if (adminRole == null)
                    {
                        adminRole = new Role { Name = "Administrator" };
                        await _unitOfWork.Roles.AddAsync(adminRole);
                        await _unitOfWork.CompleteAsync();
                    }

                    var adminUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == "admin");
                    if (adminUser == null)
                    {
                        await _authService.RegisterAsync(
                            "admin",
                            "admin",
                            "admin@servercraft.com",
                            "System",
                            "Administrator"
                        );

                        adminUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Username == "admin");
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