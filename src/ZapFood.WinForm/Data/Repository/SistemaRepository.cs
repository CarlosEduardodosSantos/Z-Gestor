using System.Linq;
using Dapper;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Repository
{
    public class SistemaRepository : BaseRepository
    {
        public SistemaEnum ObterSistema()
        {
            using (var conn = Connection)
            {
                
                conn.Open();
                var sistema = conn.Query<string>("Select Top 1 Isnull(NOME_APLICATIVO,'ETICKET') from Versao")
                    .FirstOrDefault();
                conn.Close();

                switch (sistema)
                {
                    case "ETICKET":
                        return SistemaEnum.Eticket;
                    case "ORDER":
                        return SistemaEnum.Order;
                    case "PHARM":
                        return SistemaEnum.Pharm;
                    case "SHOP":
                        return SistemaEnum.Shop;
                    default:
                        return SistemaEnum.Eticket;
                }
            }
        }
    }
}