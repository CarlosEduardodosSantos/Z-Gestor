﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapFood.WinForm.DeliveryApp.Enum
{
    public enum OrderStatus
    {
        NovoPedido = 0,
        Confirmado = 1,
        Entregue = 2,
        Cancelado = 3,
        Enviado = 4,
        CanceladoAutomaticamenteSistema = 5,
        CanceladoComPagamentoEstornadoRestaurante = 6,
        CanceladoComPagamentoEstornadoSistema = 7,
        AguardandoAprovacaoDoPagamentoOnLine = 8,
        PagamentoOnLineReprovado = 9
    }
}
