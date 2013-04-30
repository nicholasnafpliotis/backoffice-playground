using Backoffice.Services;
using Backoffice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Backoffice.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Signing in
        [HttpGet]
        public ActionResult Login()
        {
            var model = new LoginViewModel();

            // Populate the previously used login name if we have one.
            if (Request.Cookies[GlobalSettings.Backoffice.LoginNameCookieName] != null) 
            {
                model.LoginName = Request.Cookies[GlobalSettings.Backoffice.LoginNameCookieName].Value;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var authService = new UserIdentityAuthenticationService();
                if(authService.SignIn(model.LoginName, model.Password))
                {
                    if(Url != null && !string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username/password. Please try again.");
                    return RedirectToAction("Login", model);
                }
            }

            ModelState.AddModelError("", "Invalid username/password. Please try again.");
            return RedirectToAction("Login", model);
        }
        #endregion

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
