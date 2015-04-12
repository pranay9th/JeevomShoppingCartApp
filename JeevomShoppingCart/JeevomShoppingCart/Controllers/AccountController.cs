using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JeevomShoppingCart.Models;
using JeevomShoppingCart.ViewModels;

namespace JeevomShoppingCart.Controllers
{
    public class AccountController : Controller
    {

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
            {
            if (ModelState.IsValid)
                {
                try
                    {
                        UserAccessViewModel.RegisterUser(model);
                        ViewBag.Message = "email has been sent to your email id. Please varify";

                    }
                catch (MembershipCreateUserException e)
                    {
                   // ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                    }
                }

            // If we got this far, something failed, redisplay form
            return View(model);
            }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
            {
            ViewBag.ReturnUrl = returnUrl;
            return View();
            }
        [AllowAnonymous]
        public ActionResult Varify(string ID,string activationToken)
        {
            if(UserAccessViewModel.ActivateUser(ID, activationToken))
            {
                ViewBag.Message = "user is activated. Please go to login page for login";
            }
            else
	        {
                ViewBag.Message = "Activation url is not valid";
	        }

            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
            {
            return View();
            }

        [AllowAnonymous]
        public ActionResult ActivateUser()
            {

              return View();
            }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
            {
            if (ModelState.IsValid )
                {
                    if (UserAccessViewModel.AuthenticateUser(model.UserName,model.Password))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                
                }
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
            }
        private ActionResult RedirectToLocal(string returnUrl)
            {
            if (Url.IsLocalUrl(returnUrl))
                {
                return Redirect(returnUrl);
                }
            else
                {
                return RedirectToAction("Index", "Home");
                }
            }
    }
}
