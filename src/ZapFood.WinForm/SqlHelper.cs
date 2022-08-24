using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm
{
    public class SqlHelper
    {
        private SqlConnection conn;

        public SqlHelper(string connectionString)
        {
            conn = new SqlConnection(connectionString);
        }

        public bool IsConected
        {
            get
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        conn.Close();

                        return true;
                    }
                }
                catch
                {
                    //ignored
                }
                return false;
            }
        }

        public void CriarTabelas()
        {
            conn.Open();
            conn.Query(Instalacao.PedidoVapVupt);
            conn.Query(Instalacao.AccessToken);
            conn.Close();
        }
    }
}