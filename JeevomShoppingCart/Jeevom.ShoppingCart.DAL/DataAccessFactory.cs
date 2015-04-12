using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jeevom.ShoppingCart.DAL
    {
    public static class DataAccessFactory
        {
        public static IDataAccess GetDAL(string connString, string strDBType)
            {
            switch (strDBType.ToUpper().Trim())
                {
                case "SQL": return (new SQLDataAccess(connString));
                default: throw new KeyNotFoundException("Invalid Database Type provided.");
                }
            }
        }
    }
