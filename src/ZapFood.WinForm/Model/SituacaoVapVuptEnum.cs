namespace ZapFood.WinForm.Model
{
    public enum SituacaoVapVuptEnum
    {
        //Indica um pedido foi colocado no sistema.
        PLACED = 1,
        //Indica um pedido confirmado.
        CONFIRMED = 2,
        //Indica um pedido que foi recebido pelo e-PDV.
        INTEGRATED = 3,
        //Indica um pedido que foi cancelado.
        CANCELLED = 4,
        //Indica um pedido que foi despachado ao cliente.
        DISPATCHED = 5,
        //Indica um pedido que foi entregue.
        DELIVERED = 6,
        //Indica um pedido que foi concluído (Em até duas horas do fluxo normal)*
        CONCLUDED = 7,
        //Indica de o pedido retornou ao destino
        ARRIVED_AT_ORIGIN = 8,

        GOING_TO_ORIGIN = 9,
        CANCELLATION_REQUESTED = 10,
        READYTODELIVER = 11,
        //Indica se o pedido é elegível para requisitar o serviço de entrega sob demanda e o custo do serviço caso seja elegível
        REQUEST_DRIVER_AVAILABILITY = 12,
        //Indica que foi feita uma requisição do serviço de entrega sob demanda
        REQUEST_DRIVER = 13,
        //Requisição de entrega aprovada
        REQUEST_DRIVER_SUCCESS = 14,
        //Requisição de entrega negada
        REQUEST_DRIVER_FAILED = 15,
        //CASO NÃO ENCONTRE UM STATUS É ENVIADO COMO OUTROS
        OUTROS = 99

    }
}