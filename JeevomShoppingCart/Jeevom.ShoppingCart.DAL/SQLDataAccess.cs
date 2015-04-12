using System;
using System.Data.SqlClient;
using System.Data;

namespace Jeevom.ShoppingCart.DAL
{
    public class SQLDataAccess : IDataAccess
    {
        public string ConnectionString { get; private set; }
        SqlConnection MyConnection;

        public SQLDataAccess(string connString)
        {
            DataBaseType = "SQL";
            ConnectionString = connString;
        }
        private SqlConnection getConnection()
        {
            if (MyConnection == null)
                MyConnection = new SqlConnection(ConnectionString);

            if (MyConnection.State == ConnectionState.Closed || MyConnection.State == ConnectionState.Broken)
                MyConnection.Open();

            return MyConnection;
        }

        #region IDataAccess Members

        public string DataBaseType { get; private set; }

        public int ExecuteNonQuery(string commandText)
        {
            try
            {
                SqlCommand comm = new SqlCommand(commandText, getConnection());
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ExecuteScalar(string commandText)
        {
            SqlCommand cmd = new SqlCommand(commandText, getConnection());
            return cmd.ExecuteScalar();
        }

        public void DisposeConnection()
        {
            if (MyConnection == null)
                return;

            if (MyConnection.State == ConnectionState.Open)
                MyConnection.Close();

            MyConnection = null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (MyConnection != null && MyConnection.State == ConnectionState.Open)
            {
                MyConnection.Close();
                MyConnection.Dispose();
            }
        }
        #endregion


        public DataSet ExecuteSelectQuery(string commandText)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(commandText, getConnection());

                SqlDataAdapter sqlDA = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlDA.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
