using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ZapFood.WinForm.Data.Repository
{
    public class BaseRepository : IDisposable
    {
        public IDbConnection Connection => new SqlConnection(ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString);

        public void Dispose()
        {
            
            GC.SuppressFinalize(this);
        }
    }
}