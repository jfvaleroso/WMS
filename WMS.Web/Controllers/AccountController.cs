using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WMS.Web.Models;

namespace WMS.Web.Controllers
{
    public class AccountController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
            {

                string[] roles = Roles.GetRolesForUser(model.UserName);
                if (roles.Any())
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                return RedirectToAction("AccessDenied", "Error");
            }

            // If we got this far, something failed, redisplay for
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }
        #endregion


        #region Logoff
        //[HttpPost]
        public ActionResult LogOff()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");

        }
        #endregion

        #region Helpers
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
        #endregion


    }
}
