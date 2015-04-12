using System;
using System.Web;
using JeevomShoppingCart.Models;
using System.Net.Mail;
using Jeevom.ShoppingCart.DAL;
using System.Configuration;
using System.Net;
using System.Data;
using System.Collections;
using JeevomShoppingCard.Models;
using System.Collections.Generic;

namespace JeevomShoppingCart.ViewModels
{
    public class UserAccessViewModel
    {
        static string ConnectionString;
        
        static UserAccessViewModel() {
            ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        }

        public static IList<Product> GetAllProducts()
        {

            IList<Product> allProducts = new List<Product>();

            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                String query = "SELECT * from dbo.JeevomProduct";

                try
                {
                    DataSet result = db.ExecuteSelectQuery(query);
                    foreach (DataRow dr in result.Tables[0].Rows)
                    {
                        allProducts.Add(new Product { 
                        ProductID=Convert.ToInt32(dr["ProductID"]),
                        ProductName = Convert.ToString(dr["ProductName"]),
                        Price=Convert.ToDecimal(dr["Price"])
                        });
                    }
                 
                }
                catch (Exception ex)
                {
                   //Exception handling
                }
            }
            return allProducts;
        }

        public static bool AddProductToUserCart(int UserID, Product selectedProduct)
        {
            bool IsProductAdded = false;
            int ShoppingCartID=-1;
        
            //Step 1: Based on user Id get current shopping Cart Id.
            String query = "SELECT * from dbo.UserShoppingCart where UserID =" + UserID ;
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                try
                {
                    DataSet result = db.ExecuteSelectQuery(query);
                    ShoppingCartID = GetOrderPendingShoppingCartID(result.Tables[0]);

                    if (ShoppingCartID == -1)//new cart need to be inserted
                    {
                        ShoppingCartID = InsertNewShoppingCartForUser(UserID);
                    }
                    IsProductAdded = AddProductOrderToCart(ShoppingCartID, selectedProduct);
                }
                catch (Exception ex)
                {

                }
                //Step 2: Check status of shopping cart if canceled then delete (also delete all shopping cart enery in cart detail table) and create new cart entry.
                //Step 3: Based on shopping cart Id insert or update shopping cart detail table.
            }
            return IsProductAdded;
        }

        public static bool AddProductOrderToCart(int CartId,Product pd) {
            String query = "INSERT INTO dbo.ShoppingCartDetails (ShoppingCartID,ProductID,UnitCount) VALUES (" + CartId + "," + pd.ProductID + "," + pd.UnitCount + ")";
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                try
                {
                    db.ExecuteNonQuery(query);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }


        public static int GetOrderPendingShoppingCartID(DataTable userCartInfo) {
            int ShoppingCartID=-1;
            foreach (DataRow dr in userCartInfo.Rows)
            {
                if (!Convert.ToBoolean(dr["IsOrderPlaced"]))
                {
                    ShoppingCartID = Convert.ToInt32(dr["ShoppingCartID"]);
                }
            }
            return ShoppingCartID;
        }

        public static int InsertNewShoppingCartForUser(int UserID){

            int newShoppingCartID = -1;
            string query;

            query = "INSERT INTO dbo.UserShoppingCart (UserId,IsOrderPlaced) VALUES (" + UserID + ",'false'" + ")";
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                try
                {
                    db.ExecuteNonQuery(query);

                    query = "SELECT * from dbo.UserShoppingCart where UserID =" + UserID + " and IsOrderPlaced='false'";

                    DataSet result = db.ExecuteSelectQuery(query);
                    newShoppingCartID = Convert.ToInt32(result.Tables[0].Rows[0]["ShoppingCartID"]);
                }
                catch (Exception ex){
                
                }  
            }
            
            return newShoppingCartID;
        }
        public static void RegisterUser(RegisterModel model)
        {
           
            var activationcode = Guid.NewGuid().ToString();
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {

                String query = "INSERT INTO dbo.UserInfo (FirstName,LastName,Password,Email,ActivationToken,IsAccountActive) VALUES ('" + model.UserName + "','','" + model.Password + "','" + model.Email + "','" + activationcode + "','false'" + ")";
                try
                {
                    db.ExecuteNonQuery(query);

                    SendMailWithActivationURL(new UserInfo
                    {
                        ActivationToken = activationcode,
                        Email = model.Email,
                        Name = model.UserName
                    });
                }
                catch (Exception ex)
                {
                    //
                }

            }
        }

        public static bool ActivateUser(string id,string activationToken){
            bool activationStatus=false;
            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                String query = "SELECT * from dbo.userInfo where FirstName ='"+id+"' and ActivationToken='"+activationToken+"'";

                try
                {
                   DataSet result= db.ExecuteSelectQuery(query);
                   if (result.Tables[0].Rows.Count>0) {
                       query = "UPDATE dbo.userInfo SET IsAccountActive = 'true' Where ActivationToken='" + activationToken+"'";
                       db.ExecuteNonQuery(query);
                       activationStatus = true;
                   }
                   else
                   {
                       activationStatus = false;
                   }
                }
                catch (Exception ex)
                {
                    activationStatus = false;
                  //
                }
            }
            return activationStatus;
        }

        public static bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            using (var db = DataAccessFactory.GetDAL(ConnectionString, "SQL"))
            {
                String query = "SELECT * from dbo.userInfo where FirstName ='" + username + "' and Password='" + password + "'";

                try
                {
                    DataSet result = db.ExecuteSelectQuery(query);
                    if (result.Tables[0].Rows.Count > 0)
                    {
                        var IsAccountActive = Convert.ToBoolean(result.Tables[0].Rows[0]["IsAccountActive"]);
                        if (IsAccountActive)
                        {
                            isAuthenticated = true;
                            HttpContext.Current.Session.Add("IsValidSession", true);
                            HttpContext.Current.Session["UserName"] = username;
                            HttpContext.Current.Session["UserID"] = result.Tables[0].Rows[0]["UserId"];
                        }
                        else
                            isAuthenticated = false;
                    }
                    else
                    {
                        isAuthenticated = false;
                    }
                }
                catch (Exception ex)
                {
                    isAuthenticated = false;
                    //
                }
                
                return isAuthenticated;
            }
        }

        public static void SendMailWithActivationURL(UserInfo user)
        {

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("noreply@gmail.com");
                message.To.Add(user.Email);
                message.Subject = "Account Activation";
                string body = "Hello " + user.Name.Trim() + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/account/Varify?ID=" + user.Name + "&activationToken=" + user.ActivationToken;
                body += "<br /><br />Thanks";
                message.Body = body;
                message.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                NetworkCredential networkCredentials = new NetworkCredential("noreply@gmail.com", "accountpassword");
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredentials;
                smtpClient.Port = 587;
                smtpClient.Send(message);
            }
        }
    }
}