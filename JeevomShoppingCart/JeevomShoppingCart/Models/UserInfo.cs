using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JeevomShoppingCart.Models
    {
    public class UserInfo
        {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ActivationToken { get; set; }
        public string IsAccountActive { get; set; }
        }
    }