using System;
using System.Linq;
using System.Text;
using Dapper;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm.Data.Repository
{
    public class InstalacaoRepository : BaseRepository
    {
        public bool VerificaInstalacao()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var result = conn.Query<bool>(Instalacao.VerificaInstalacao).FirstOrDefault();
                conn.Close();

                return result;
            }
        }

        public VapVupVersao GetVersao()
        {
            using (var conn = Connection)
            {
                
                try
                {
                    conn.Open();
                    var versao = conn.Query<VapVupVersao>("select * from VapVupVersao").FirstOrDefault();
                    conn.Close();
                    return versao;
                    
                }
                catch
                {
                    return null;
                }
                
                

                
            }
        }

        public bool Update()
        {
            using (var conn = Connection)
            {
                conn.Open();
                var result = conn.Query<bool>(Instalacao.ScriptUpdate).FirstOrDefault();
                conn.Close();

                return result;
            }
        }
        public void DefineVersao(VapVupVersao versao)
        {
            var sql =
                "Update VapVupVersao Set VersaoAtual = @VersaoAtual, DataAtualizacao = @DataAtualizacao Where VersaoId = @VersaoId";
            var parms = new DynamicParameters();
            parms.Add("@VersaoId", versao.VersaoId);
            parms.Add("@VersaoAtual", versao.VersaoAtual);
            parms.Add("@DataAtualizacao", versao.DataAtualizacao);

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql, parms);
                conn.Close();
            }
        }

        public void CriarTabelas()
        {
            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(Instalacao.PedidoVapVupt);
                conn.Query(Instalacao.AccessToken);
                conn.Close();
            }
        }
        public void InsertVersao(VapVupVersao versao)
        {
            var sql = new StringBuilder();
            sql.AppendLine("Insert Into VapVupVersao(VersaoId, DataInstalacao, VersaoAtual, DataAtualizacao)");
            sql.AppendLine("Values(@VersaoId, @DataInstalacao, @VersaoAtual, @DataAtualizacao)");

            var parms = new DynamicParameters();
            parms.Add("@VersaoId", versao.VersaoId);
            parms.Add("@DataInstalacao", versao.DataInstalacao);
            parms.Add("@VersaoAtual", versao.VersaoAtual);
            parms.Add("@DataAtualizacao", versao.DataAtualizacao);

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(Instalacao.CreateTableVapVupVersao);
                conn.Query(sql.ToString(), parms);
                conn.Close();
            }
        }
    }
}