using System;
using System.Windows.Forms;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Layout
{
    public partial class RowDeliveryApp : UserControl
    {
        public string PedidoId
        {
            set { lbNroPedido.Text = "Nro APP: " + value; }
        }
        public string Cliente
        {
            set { lbCliente.Text = "Cliente: " + value; }
        }
        public string Endereco
        {
            set { lbEndereco.Text = "Endereço: " + value; }
        }

        public string Telefone
        {
            set { lbFone.Text = "Fone: " + value; }
        }
        public string DataHora
        {
            set
            {
                lbDataHora.Text = $"Data: { value }";
            }
        }
        public string DataAgendamento
        {
            set
            {
                lbAgendamento.Text = !string.IsNullOrEmpty(value) ? $"Agendamento: { value }" : "";
            }
        }

        public string Tipo
        {
            set {
                var tipo = value == "DELIVERY" ? "Entregar" : value == "TAKEOUT" ? "Retirar" : "Mesa";
                lbTipo.Text = "Tipo: " + tipo; }
        }
        public string Status
        {
            set
            {
                switch (value)
                {
                    case "CONFIRMED":
                        lbStatus.Text = "Aguardando entragar";
                        break;
                    case "INTEGRATED":
                        lbStatus.Text = "Aguardando confirmação";
                        break;
                    case "CANCELLED":
                        lbStatus.Text = "Pedido cancelado";
                        break;
                    case "DISPATCHED":
                        lbStatus.Text = "Pedido sai pra entrega";
                        break;
                    case "DELIVERED":
                        lbStatus.Text = "Pedido entregue";
                        break;
                    case "CONCLUDED":
                        lbStatus.Text = "Pedido concluido";
                        break;
                    default:
                        break;
                }
            }
        }
        
        public string ButtonText
        {
            set => button1.Text = value;
        }
        public object ItemSource { get; set; }

        public RowDeliveryApp()
        {
            InitializeComponent();
            button1.Click += new EventHandler(CConfirmEvent);
        }

        public event EventHandler<EventArgs> ConfirmEvent;
        void CConfirmEvent(object sender, EventArgs e)
        {
            var completedEvent = ConfirmEvent;
            completedEvent?.Invoke(this, e);
        }
    }
}
