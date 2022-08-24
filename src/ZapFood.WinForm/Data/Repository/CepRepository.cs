using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Repository
{
    public class CepRepository : BaseRepository
    {
        public void Adicionar()
        {
            var r = new StreamReader("D:\\Uteis\\CEP.json");
            var myJson = r.ReadToEnd();

            var data = JsonConvert.DeserializeObject<RootCep>(myJson);

            using (var conn = Connection)
            {

                var sql = new StringBuilder();
                sql.AppendLine(
                    "Insert Into CepBrasil(objectId, CEP, cidade, estado, bairro, logradouro, numero, createdAt, updatedAt)");
                sql.AppendLine(
                    "Values (@objectId, @CEP, @cidade, @estado, @bairro, @logradouro, @numero, @createdAt, @updatedAt)");

                conn.Open();

                foreach (var cep in data.results)
                {
                    var parms = new DynamicParameters();
                    parms.Add("@objectId", cep.objectId);
                    parms.Add("@CEP", cep.CEP);
                    parms.Add("@cidade", cep.cidade);
                    parms.Add("@estado", cep.estado);
                    parms.Add("@bairro", cep.bairro);
                    parms.Add("@logradouro", cep.logradouro);
                    parms.Add("@numero", cep.numero);
                    parms.Add("@createdAt", cep.createdAt);
                    parms.Add("@updatedAt", cep.updatedAt);

                    conn.Query(sql.ToString(), parms);

                }

                conn.Close();
            }
        }

        public List<Cep> OnterPorCidade(string estado, string cidade)
        {
            var sql = "Select * From CepBrasil Where estado like @estado And cidade Like @cidade";
            using (var conn = Connection)
            {
                conn.Open();
                var ceps = conn.Query<Cep>(sql, new {estado, cidade}).ToList();
                conn.Close();

                return ceps;
            }
        }

        public List<Estado> OnterEstados()
        {
            var sql = "Select estado From CepBrasil Group By estado";
            using (var conn = Connection)
            {
                conn.Open();
                var estados = conn.Query<Estado>(sql).ToList();
                conn.Close();

                return estados;
            }
        }

        public List<Cidade> OnterCidadePorEstado(string estado)
        {
            var sql = "Select estado, cidade From CepBrasil Where estado like @estado Group By estado, cidade";
            using (var conn = Connection)
            {
                conn.Open();
                var cidades = conn.Query<Cidade>(sql, new { estado }).ToList();
                conn.Close();

                return cidades;
            }
        }

        public List<Bairro> OnterBairroPorCidade(string estado, string cidade)
        {
            var sql = "Select  estado, cidade, bairro, faixaInicialBairro, faixaFinalBairro From CepBrasil Where estado like @estado And cidade Like @cidade Group By estado, cidade, bairro, faixaInicialBairro, faixaFinalBairro";
            using (var conn = Connection)
            {
                conn.Open();
                var bairros = conn.Query<Bairro
                    >(sql, new { estado, cidade }).ToList();
                conn.Close();

                return bairros;
            }
        }
    }

  
}