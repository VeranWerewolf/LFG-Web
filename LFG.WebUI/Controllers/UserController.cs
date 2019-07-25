using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.Domain.Infrastructure;
using LFG.WebUI.Infrastructure;
using LFG.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LFG.WebUI.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.User.Identity.Name);
            }
        }
        private Dictionary<string, object> GetData(string actionName)
        {
            string role = "";
            if (HttpContext.User.IsInRole("Users"))
            { role = "Users"; }
            else if (HttpContext.User.IsInRole("Moderators"))
            { role = "Moderators"; }
            else if (HttpContext.User.IsInRole("Administrators"))
            { role = "Administrators"; }
            Dictionary<string, object> dict = new Dictionary<string, object>();

            dict.Add("Action", actionName);
            dict.Add("Пользователь", HttpContext.User.Identity.Name);
            dict.Add("Аутентифицирован?", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Тип аутентификации", HttpContext.User.Identity.AuthenticationType);
            dict.Add("В какой роли?", role);

            return dict;
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public ActionResult Index()
        {
            return RedirectToAction("UserInfo");
        }

        [AllowAnonymous]
        public ViewResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserCreationModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result =
                    await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    TempData["message"] = string.Format("Добро пожаловать, \"{0}\"!", user.UserName);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = string.Format("Ошибка при регистрации");
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("/");
            }

            if (returnUrl == null)
            { ViewBag.returnUrl = "/"; }
            else
            { ViewBag.returnUrl = returnUrl; }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel details, string returnUrl)
        {
            AppUser user = await UserManager.FindAsync(details.Name, details.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Некорректное имя или пароль.");
            }
            else
            {
                ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);

                ident.AddClaims(LocationClaimsProvider.GetClaims(ident));

                AuthManager.SignOut();
                AuthManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);
                return Redirect(returnUrl);
            }
            return View(details);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult GoogleLogin(string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleLoginCallback",
                    new { returnUrl = returnUrl })
            };

            HttpContext.GetOwinContext().Authentication.Challenge(properties, "Google");
            return new HttpUnauthorizedResult();
        }
        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback(string returnUrl)
        {
            ExternalLoginInfo loginInfo = await AuthManager.GetExternalLoginInfoAsync();
            AppUser user = await UserManager.FindByEmailAsync(loginInfo.Email);
            if (user == null)
            {
                user = new AppUser
                {
                    Email = loginInfo.Email,
                    UserName = loginInfo.DefaultUserName,
                };

                IdentityResult result = await UserManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
                else
                {
                    result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
            }

            ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user,
                DefaultAuthenticationTypes.ApplicationCookie);

            ident.AddClaims(loginInfo.ExternalIdentity.Claims);

            AuthManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = false
            }, ident);

            return Redirect(returnUrl ?? "/");
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("List", "Activity");
        }

        public ActionResult UserInfo()
        {
            return View(GetData("UserInfo"));
        }

        public ActionResult CreateActivity()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserCreationModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result =
                    await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }
    }
}