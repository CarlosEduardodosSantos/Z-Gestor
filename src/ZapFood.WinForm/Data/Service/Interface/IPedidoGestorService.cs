using System;
using System.Collections.Generic;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Service.Interface
{
    public interface IPedidoGestorService : IDisposable, IDataServiceBase<PedidoVapVupt>
    {
        IEnumerable<PedidoVapVupt> ObterPorData(DateTime dataInicio, DateTime dataFinal);
        IEnumerable<PedidoVapVupt> ObterPorAppId(string vendaId);
        IEnumerable<PedidoVapVupt> ObterNovos();
        IEnumerable<PedidoVapVupt> ObterPorSituacao(SituacaoVapVuptEnum situacao);
        IEnumerable<PedidoVapVupt> ObterPedidosEntregando();
        IEnumerable<PedidoVapVupt> ObterPedidosEntreges();
        IEnumerable<PedidoVapVupt> ObterPedidosRetirar();
        PedidoVapVupt ObterPorPedidoId(string pedidoId);
    }
}