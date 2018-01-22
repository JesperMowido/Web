using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BC.Web.Models;
using Core.Infrastructure;
using Core.Entities;
using Core.Accounts;

namespace BC.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private UserManager<User, int> _userManager;
        private BillionCompanyDbContext _context = BillionCompanyDbContext.Create();

        public AccountController()
        {
            var userManager = new UserManager<User, int>(new UserStoreService(new BillionCompanyDbContext()) { });
            userManager.PasswordHasher = new BCPasswordHasher();
            userManager.UserValidator = new CustomUserValidator<User>();
            _userManager = userManager;
        }

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public UserManager<User, int> UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager<User, int>>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                var user = UserManager.FindByName<User, int>(model.Email);
                var roles = user.Roles.Select(r => r.Name);

                if (string.IsNullOrEmpty(returnUrl))
                {
                    //if (roles.Contains(CustomerRoles.Parliv.Couple))
                    //{
                    //    returnUrl = "par";
                    //}
                    //else if (roles.Contains(CustomerRoles.Parliv.Admin))
                    //{
                    //    returnUrl = "admin";
                    //}
                    //else if (roles.Contains(CustomerRoles.Parliv.Consultant))
                    //{
                    //    returnUrl = "parkonsult";
                    //}

                    returnUrl = "/"; //TEMP
                }

            }

            switch (result)
            {
                case SignInStatus.Success:
                    return Redirect(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Det gick inte att logga in.");
                    return View(model);
            }
        }

        //[Authorize(Roles = CustomerRoles.Parliv.Admin + "," + CustomerRoles.Parliv.Consultant)]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();

            //if (User.IsInRole(CustomerRoles.Parliv.Consultant))
            //{
            //    model.RoleName = CustomerRoles.Parliv.Couple;
            //}

            //var roles = _context.RoleRepository.GetAll().ToList();
            //model.Roles = new SelectList(roles, "Name", "Name");

            model.PageTitle = "Mowido - Register";

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = CustomerRoles.Parliv.Admin + "," + CustomerRoles.Parliv.Consultant)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, Password = UserManager.PasswordHasher.HashPassword(model.Password), InsertDate = DateTime.Now };

                var result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "Web"); // TEMP

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //if (User.IsInRole("Consultant"))
                    //{
                    //    return RedirectToAction("Index", "Consultant");
                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", "Admin");
                    //}

                    return Redirect("/");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
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
    }
}