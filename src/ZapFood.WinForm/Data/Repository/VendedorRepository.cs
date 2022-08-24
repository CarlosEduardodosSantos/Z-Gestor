using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Repository
{
    public class VendedorRepository : BaseRepository
    {
        public IEnumerable<VendedorModel> ObterTodos()
        {
            var sql = new StringBuilder().AppendLine("Select * From VW_VapVuptUsuario");

            using (var conn = Connection)
            {
                try
                {
                    conn.Open();
                    var result = conn.Query<VendedorModel>(sql.ToString());
                    conn.Close();

                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
        }

    }
}