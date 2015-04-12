using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using JeevomShoppingCard.Models;
using JeevomShoppingCart.ViewModels;

namespace JeevomShoppingCart.Controllers
{
    public class ShoppingCartController : Controller
    {
        public ActionResult ShoppingCart()
        {
            if (System.Web.HttpContext.Current.Session == null && System.Web.HttpContext.Current.Session["IsValidSession"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.userName = System.Web.HttpContext.Current.Session["UserName"].ToString();
                int userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                var shoppingCart=ShoppingCartViewModel.GetUserCartInfo(userID);

                if (shoppingCart==null)
                {
                    ViewBag.message = "User Don't have any unordered shoping cart";
                }
                return View(shoppingCart);
            }
        }

        public JsonResult PlaceOrder()
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["IsValidSession"] != null)
            {
                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                bool result = ShoppingCartViewModel.PlaceOrder(userID);

                return Json(result,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult CancelCart()
        {

            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["IsValidSession"] != null)
            {

                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                bool result = ShoppingCartViewModel.CancelCart(userID);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
    }
}
