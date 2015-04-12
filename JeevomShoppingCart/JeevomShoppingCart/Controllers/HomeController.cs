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
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.Session == null && System.Web.HttpContext.Current.Session["IsValidSession"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.userName = System.Web.HttpContext.Current.Session["UserName"].ToString();
                return View();
            }
        }

        public JsonResult GetAllProducts()
        {
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["IsValidSession"] != null)
            {
                IList<Product> result = UserAccessViewModel.GetAllProducts();

                return Json(result,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult AddProductToUserCart(Product selectedProduct) {

            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["IsValidSession"] != null)
            {

                var userID = Convert.ToInt32(System.Web.HttpContext.Current.Session["UserID"]);
                var IsAdded = UserAccessViewModel.AddProductToUserCart(userID, selectedProduct);
              return Json(IsAdded, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
    }
}
