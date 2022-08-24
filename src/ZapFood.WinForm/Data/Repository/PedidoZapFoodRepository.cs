using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Repository
{
    public class PedidoZapFoodRepository : BaseRepository
    {
        private void Adicionar(PedidoVapVupt pedido)
        {
            var sql = new StringBuilder().AppendLine("Insert Into PedidoVapVupt (");
            sql.AppendLine("VapVuptId, PedidoId, VendaId, DataHora, Situacao, FileJson, Aplicacao, TipoPedido) Values (");
            sql.AppendLine("@VapVuptId, @PedidoId, @VendaId, @DataHora, @Situacao, @FileJson, @Aplicacao, @TipoPedido)");

            var parans = new DynamicParameters();
            parans.Add("@VapVuptId", pedido.VapVuptId);
            parans.Add("@PedidoId", pedido.PedidoId);
            parans.Add("@VendaId", pedido.VendaId);
            parans.Add("@DataHora", pedido.DataHora);
            parans.Add("@Situacao", (int)pedido.Situacao);
            parans.Add("@FileJson", pedido.FileJson);
            parans.Add("@Aplicacao", (int)pedido.Aplicacao);
            parans.Add("@TipoPedido", (string)pedido.TipoPedido);

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql.ToString(), parans);
                conn.Close();
            }
        }
        private void Atualizar(PedidoVapVupt pedido)
        {
            var sql = new StringBuilder().AppendLine("Update PedidoVapVupt Set");
            sql.AppendLine("VendaId = @VendaId,");
            sql.AppendLine("DataHora = @DataHora,");
            sql.AppendLine("Situacao = @Situacao,");
            sql.AppendLine("FileJson = @FileJson,");
            sql.AppendLine("Aplicacao = @Aplicacao,");
            sql.AppendLine("TipoPedido = @TipoPedido");
            sql.AppendLine("Where PedidoId = @PedidoId");

            var parans = new DynamicParameters();
            parans.Add("@PedidoId", pedido.PedidoId);
            parans.Add("@VendaId", pedido.VendaId);
            parans.Add("@DataHora", pedido.DataHora);
            parans.Add("@Situacao", (int)pedido.Situacao);
            parans.Add("@FileJson", pedido.FileJson);
            parans.Add("@Aplicacao", (int)pedido.Aplicacao);
            parans.Add("@TipoPedido", (string)pedido.TipoPedido);

            using (var conn = Connection)
            {
                conn.Open();
                conn.Query(sql.ToString(), parans);
                conn.Close();
            }
        }
        public void AddOrUpdate(PedidoVapVupt pedido)
        {
            using (var conn = Connection)
            {
                conn.Open();
                var existes = conn.Query<PedidoVapVupt>("Select * From PedidoVapVupt Where PedidoId = @PedidoId", new { pedido.PedidoId }).Any();
                conn.Close();

                if (existes)
                    Atualizar(pedido);
                else
                    Adicionar(pedido);
            }
        }
        public IEnumerable<PedidoVapVupt> ObterPorData(DateTime dataInicio, DateTime dataFinal)
        {
            var sql = "Select * From PedidoVapVupt Where Cast(DataHora as Date) Between @dataInicio And @dataFinal";
            using (var conn = Connection)
            {
                conn.Open();
                var pedidos = conn.Query<PedidoVapVupt>(sql, new { dataInicio, dataFinal });
                conn.Close();

                return pedidos;
            }
        }
        public IEnumerable<PedidoVapVupt> ObterPorAppId(string vendaId)
        {
            var sql = "Select * From PedidoVapVupt Where VendaId = @vendaId";
            using (var conn = Connection)
            {
                conn.Open();
                var pedido = conn.Query<PedidoVapVupt>(sql, new { vendaId });
                conn.Close();

                return pedido;
            }
        }
        public IEnumerable<PedidoVapVupt> ObterNovos()
        {
            var sql = "Select * From PedidoVapVupt Where  Situacao In (1,3)";
            using (var conn = Connection)
            {
                conn.Open();
                var pedidos = conn.Query<PedidoVapVupt>(sql);
                conn.Close();

                return pedidos;
            }
        }
        public IEnumerable<PedidoVapVupt> ObterPorSituacao(SituacaoVapVuptEnum situacao)
        {
            var sql = "Select * From PedidoVapVupt Where Cast(DataHora as Date) = Cast(Getdate() as Date)  And Situacao = @situacao";
            using (var conn = Connection)
            {
                conn.Open();
                var pedidos = conn.Query<PedidoVapVupt>(sql, new { situacao });
                conn.Close();

                return pedidos;
            }
        }
        public PedidoVapVupt ObterPorPedidoId(string pedidoId)
        {   
            
            var sql = "Select * From PedidoVapVupt Where PedidoId = @pedidoId";
            using (var conn = Connection)
            {
                conn.Open();
                var pedido = conn.Query<PedidoVapVupt>(sql, new { pedidoId }).FirstOrDefault();
                conn.Close();

                return pedido;
            }
            
        }
        public IEnumerable<PedidoVapVupt> ObterPedidosEntregando()
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select * From Vw_GestorPedidosEntregando");
            using (var conn = Connection)
            {
                conn.Open();
                var pedidos = conn.Query<PedidoVapVupt>(sql.ToString());
                conn.Close();

                return pedidos;
            }
        }
        public IEnumerable<PedidoVapVupt> ObterPedidosEntreges()
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select * From Vw_GestorPedidosEntreges");
            using (var conn = Connection)
            {
                conn.Open();
                var pedidos = conn.Query<PedidoVapVupt>(sql.ToString());
                conn.Close();

                return pedidos;
            }
        }
        public IEnumerable<PedidoVapVupt> ObterPedidosRetirar()
        {
            var sql = new StringBuilder();
            sql.AppendLine("Select * From Vw_GestorPedidosRetirar");
            using (var conn = Connection)
            {
                conn.Open();
                var pedidos = conn.Query<PedidoVapVupt>(sql.ToString());
                conn.Close();

                return pedidos;
            }
        }
    }
}