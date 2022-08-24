using System;
using System.Collections.Generic;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Service
{
    public interface IBaseService : IDisposable
    {
        List<PollingModel> Obterpolling();
        bool PollingAcknowledgment(List<PollingModel> polling);
        PedidoRootModel ObterById(string id);
        string ObterJsonPedidoById(string id);
        bool AlterarStatusPedido(string rederence, string status, string motivo = "");
        
    }
}