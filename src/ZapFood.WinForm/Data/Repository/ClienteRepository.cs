using System.Linq;
using System.Text;
using Dapper;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm.Data.Repository
{
    public class ClienteRepository : BaseRepository
    {
        public void Adicionar(Cliente cliente, Endereco endereco)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Insert Into Televenda_2 (Fone,nome,endereco,cpf,bairro,obs1,obs2,obs3)");
            sql.AppendLine("Values");
            sql.AppendLine("(@Fone,@nome,@endereco,@cpf,@bairro,@obs1,@obs2,@obs3)");

            var parns = new DynamicParameters();
            parns.Add("@Fone", cliente.Telefone);
            parns.Add("@nome", cliente.Nome);
            parns.Add("@endereco", endereco.Rua);
            parns.Add("@cpf", "");
            parns.Add("@bairro", endereco.Bairro);
            parns.Add("@obs1", endereco.Complemento);
            parns.Add("@obs2", endereco.Referencia);
            parns.Add("@obs3", "");

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql.ToString(), parns);
                conn.Close();
            }

        }
        public void Editar(Cliente cliente, Endereco endereco)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Update Televenda_2  Set");
            sql.AppendLine("Fone = @Fone");
            sql.AppendLine("nome = @nome");
            sql.AppendLine("endereco = @endereco");
            sql.AppendLine("cpf = @cpf");
            sql.AppendLine("bairro = @bairro");
            sql.AppendLine("obs1 = @obs1");
            sql.AppendLine("obs2 = @obs2");
            sql.AppendLine("obs3 = @obs3");
            sql.AppendLine("Where Codigo = Codigo");

            var parns = new DynamicParameters();
            parns.Add("@Fone", cliente.Telefone);
            parns.Add("@nome", cliente.Nome);
            parns.Add("@endereco", endereco.Rua);
            parns.Add("@cpf", "");
            parns.Add("@bairro", endereco.Bairro);
            parns.Add("@obs1", endereco.Complemento);
            parns.Add("@obs2", endereco.Referencia);
            parns.Add("@obs3", "");

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql.ToString(), parns);
                conn.Close();
            }

        }
        public Cliente ObterClienteFone(string fone)
        {
            var sql = "Select * From Televenda_2 Where Fone Like @fone";
            using (var conn = Connection)
            {
                conn.Open();
                var cliente = conn.Query<Cliente>(sql, new {fone}).FirstOrDefault();
                conn.Close();

                return cliente;
            }
        }
    }
}