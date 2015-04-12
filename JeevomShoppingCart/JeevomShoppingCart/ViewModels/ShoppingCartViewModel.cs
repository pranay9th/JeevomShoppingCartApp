using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JeevomShoppingCard.Models;
using Jeevom.ShoppingCart.DAL;
using System.Configuration;
using System.Data;

namespace JeevomShoppingCart.ViewModels
{
    public class ShoppingCartViewModel
    {
        static string ConnectionString;

        static ShoppingCartViewModel()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public static ShoppingCart GetUserCartInfo(int UserID)
        {
            int ShoppingCartID = -1;
            DataSet result, allProducts;
            var UserShoppingCart = new ShoppingCart();
            String query = "SELECT * from dbo.UserShoppingCart where UserID =" + UserID;
            String allProductQuery = "SELECT * from dbo.JeevomProduct";
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                try
                {
                    result = db.ExecuteSelectQuery(query);
                    allProducts = db.ExecuteSelectQuery(allProductQuery);

                    ShoppingCartID = UserAccessViewModel.GetOrderPendingShoppingCartID(result.Tables[0]);

                    UserShoppingCart.ShoppingCartId = ShoppingCartID;
                    if (ShoppingCartID != -1)
                    {
                        query = "SELECT * from dbo.ShoppingCartDetails where ShoppingCartID =" + ShoppingCartID;
                        result = db.ExecuteSelectQuery(query);

                        var CartProductInfo = from cartProduct in result.Tables[0].AsEnumerable()
                                              join
                                                  productDetails in allProducts.Tables[0].AsEnumerable()
                                                  on Convert.ToInt32(cartProduct["ProductID"]) equals Convert.ToInt32(productDetails["ProductID"])
                                              select new Product
                                              {
                                                  ProductID = Convert.ToInt32(cartProduct["ProductID"]),
                                                  ProductName = Convert.ToString(productDetails["ProductName"]),
                                                  Price = Convert.ToDecimal(productDetails["Price"]),
                                                  UnitCount = Convert.ToInt32(cartProduct["UnitCount"])
                                              };
                        UserShoppingCart.CartItems = CartProductInfo.ToList();
                        UserShoppingCart.CartTotalPrice = CartProductInfo.Sum(p => p.UnitCount * p.Price);

                    }
                }
                catch (Exception ex)
                {

                }
            }

            return UserShoppingCart;
        }

        //public static void PlaceOrder(int UserID) {
        //    DataSet result;
        //    int ShoppingCartID = -1;
        //    String query = "SELECT * from dbo.UserShoppingCart where UserID =" + UserID;
        //    using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
        //    {
        //        try
        //        {

        //        result = db.ExecuteSelectQuery(query);
        //        ShoppingCartID = UserAccessViewModel.GetOrderPendingShoppingCartID(result.Tables[0]);
        //        query = "UPDATE dbo.UserShoppingCart SET IsOrderPlaced = 'true' Where UserID =" + UserID + " and ShoppingCartID=" + ShoppingCartID;

        //        db.ExecuteNonQuery(query);

        //        }
        //        catch (Exception)
        //        {
        //           // 

        //        }

        //    }
        //}

        public static bool PlaceOrder(int UserID)
        {
            var isOrderPlaced = false;
            DataSet result;
            int ShoppingCartID = -1;
            String query = "UPDATE dbo.UserShoppingCart SET IsOrderPlaced = 'true' Where UserID =" + UserID + " and IsOrderPlaced='false'"; // it should be based on shoping cart id
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                try
                {
                    result = db.ExecuteSelectQuery(query);
                    ShoppingCartID = UserAccessViewModel.GetOrderPendingShoppingCartID(result.Tables[0]);

                    db.ExecuteNonQuery(query);
                    isOrderPlaced = true;
                }
                catch (Exception)
                {
                    isOrderPlaced = false;

                }
                return isOrderPlaced;
            }
        }

        public static bool CancelCart(int UserID)
        {
            var isCartDeleted = false;
            DataSet result;
            int ShoppingCartID = -1;
            String query = "SELECT * from dbo.UserShoppingCart where UserID =" + UserID;
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                try
                {

                    result = db.ExecuteSelectQuery(query);
                    ShoppingCartID = UserAccessViewModel.GetOrderPendingShoppingCartID(result.Tables[0]);
                    query = "DELETE from dbo.UserShoppingCart Where UserID =" + UserID + " and ShoppingCartID=" + ShoppingCartID;

                    db.ExecuteNonQuery(query);

                    query = "DELETE from dbo.ShoppingCartDetails Where ShoppingCartID=" + ShoppingCartID;

                    db.ExecuteNonQuery(query);
                    isCartDeleted = true;
                }
                catch (Exception)
                {
                    isCartDeleted = false;
                }

            }
            return isCartDeleted;
        }
    }
}