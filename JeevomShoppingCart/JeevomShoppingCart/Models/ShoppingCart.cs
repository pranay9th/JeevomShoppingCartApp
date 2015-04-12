using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JeevomShoppingCard.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public IList<Product> CartItems { get; set; }
        public decimal CartTotalPrice { get; set; }
        public ShoppingCart()
        {
            ShoppingCartId = -1;
            CartItems = new List<Product>();
        }
    }
}