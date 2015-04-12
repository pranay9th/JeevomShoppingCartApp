using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeevom.ShoppingCart.DAL
{
    public interface IDataAccess:IDisposable
    {
        string DataBaseType { get; }

        DataSet ExecuteSelectQuery(string commandText);

        int ExecuteNonQuery(string commandText);

        object ExecuteScalar(string commandText);

        void DisposeConnection();
    }
}
