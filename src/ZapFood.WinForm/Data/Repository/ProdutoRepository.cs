using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm.Data.Repository
{
    public class ProdutoRepository : BaseRepository
    {
        public IEnumerable<Produto> ObterPorNomeOrId(string descricao, int produtoId)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select * from Vw_VapVuptProdutos");
            sql.AppendLine("Where (Isnull(@produtoId,0)  = 0 Or ReferenciaId = @produtoId)");
            sql.AppendLine("And (Isnull(@descricao,'') = '' Or nome Like '%'+@descricao+ '%')");
            using (var conn = Connection)
            {
                conn.Open();
                var produtos = conn.Query<Produto>(sql.ToString(), new {descricao, produtoId});
                conn.Close();

                return produtos;
            }
        }

        public bool VerificaRelacao(string id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var result = conn.Query<Produto>("Select 1 From Vw_VapVuptProdutos Where produtoId = @codigo", new { codigo = id }).Any();
                conn.Close();

                return result;
            }
        }

        public Produto ObterPorId(string id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var result = conn
                    .Query<Produto>("Select * From Vw_VapVuptProdutos Where produtoId = @codigo",
                        new {codigo = id}).FirstOrDefault();
                conn.Close();

                return result;
            }
        }

        public IEnumerable<Produto> ObterPorNome(string nome, int codigo)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var result = conn
                    .Query<Produto>("Select CODIGO as produtoId, DES_ as nome From Prod " +
                                    "Where (@nome = '' Or DES_ Like '%'+ @nome + '%') " +
                                    "And (@codigo = 0 Or Codigo = @codigo)",
                        new { nome, codigo });
                conn.Close();

                return result;
            }
        }
    }
}