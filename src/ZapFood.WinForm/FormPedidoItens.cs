using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using ZapFood.WinForm.Componente;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Data.Service;
using ZapFood.WinForm.Data.Service.Interface;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormPedidoItens : Form
    {
        private readonly PedidoRootModel _pedido;
        private readonly IBaseService _baseService;
        private bool _autoConfirma;

        public FormPedidoItens(PedidoRootModel pedido, bool autoConfirma)
        {
            _pedido = pedido;
            _autoConfirma = autoConfirma;

            if (_pedido.aplicacao == AplicacaoEnum.VapVupt)
                _baseService = new PedidoVapVuptService();
            else if (_pedido.aplicacao == AplicacaoEnum.Ifood)
                _baseService = new PedidoIfoodService();
            else if (_pedido.aplicacao == AplicacaoEnum.Neemo)
                _baseService = new PedidoDeliveyAppService();

            InitializeComponent();

            //btnEntregar.Click += Entregar_Click;
            btnRetorno.Click += Retorno_Click;
        }

        private void FormPedidoItens_Load(object sender, System.EventArgs e)
        {

            LoadObject();

            if (_autoConfirma)
                btnAceitarPedido.PerformClick();
        }

        private void LoadObject()
        {
            var situacaoPedido = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), _pedido.sitEvent, true);
            btnAceitarPedido.Enabled = situacaoPedido == SituacaoVapVuptEnum.INTEGRATED || situacaoPedido == SituacaoVapVuptEnum.PLACED;

            btnImprimir.Enabled = !btnAceitarPedido.Enabled;

            btnCancelar.Enabled = situacaoPedido == SituacaoVapVuptEnum.INTEGRATED || situacaoPedido == SituacaoVapVuptEnum.PLACED;

            btnEntregar.Enabled = situacaoPedido == SituacaoVapVuptEnum.CONFIRMED && _pedido.orderType == "DELIVERY";
            btnRetorno.Enabled = situacaoPedido == SituacaoVapVuptEnum.DISPATCHED && _pedido.orderType == "DELIVERY";
            btnRetirar.Enabled = situacaoPedido == SituacaoVapVuptEnum.CONFIRMED && _pedido.orderType != "DELIVERY";


            txtNPedido.Text = _pedido.shortReference ?? _pedido.displayId;
            txtDataHora.Text = _pedido.DataPedido.ToString("dd-MM-yyyy HH:mm");
            if (_pedido.isSchedule)
            {
                textBox1.Text = _pedido.preparationStartDateTime.ToString("dd-MM-yyyy HH:mm");
                textBox1.BackColor = Color.Khaki;
            }
            txtObservacaoPedido.Text = _pedido.observations;
            //A txt tipo apresenta confusão entre os ordertypes "TAKEOUT" e "INDOOR" devido ao app de pedidos 
            txtTipo.Text = _pedido.orderType == "DELIVERY" ? "Entregar" : _pedido.orderType == "TAKEOUT" ? "Retirar" : "Mesa";

            txtCliente.Text = _pedido.customer?.name;
            txtDocumento.Text = _pedido.customer?.documentNumber;
            txtDocumento.BackColor = _pedido.customer?.documentNumber != null ? Color.Khaki : Color.White;

            txtFone.Text = $"{_pedido.customer?.phone.number}  {_pedido.customer?.phone.localizer}";
            if (_pedido.orderType == "DELIVERY")
            {
                label4.Text = "Logradouro";
                txtRua.Text = _pedido.delivery.deliveryAddress.formattedAddress + " - " + _pedido.delivery.deliveryAddress.complement;
                txtComp.Text = _pedido.delivery.deliveryAddress.reference;
                txtBairro.Text = _pedido.delivery.deliveryAddress.neighborhood;
                txtCidade.Text = _pedido.delivery.deliveryAddress.city;
                txtEstado.Text = _pedido.delivery.deliveryAddress.state;
            }
            //orderType "INDOOR" precisa ser corrigido no app pois a relação entre "INDOOR" e "Mesa" não está correta
            if (_pedido.orderType == "INDOOR")
            {
                label4.Text = "Entregar em";
                txtRua.Text = _pedido.extraInfo;
                txtComp.Visible = false;
                txtBairro.Visible = false;
                txtCidade.Visible = false;
                txtEstado.Visible = false;

                label9.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }

            if (_pedido.orderType == "TAKEOUT")
            {
                label4.Text = "Entregar em";
                txtRua.Text = _pedido.extraInfo;
                txtComp.Visible = false;
                txtBairro.Visible = false;
                txtCidade.Visible = false;
                txtEstado.Visible = false;

                label9.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
            }

            dgvFormaPagamento.AutoGenerateColumns = false;
            dgvFormaPagamento.DataSource = _pedido.payments.methods.ToList();

            List<ComplementosDg> lista = new List<ComplementosDg>(); //sistema de complementos para pedidos Meio/Meio
            var texto = string.Empty;
            decimal valortotal = 0;
            for (int e = 0; e < _pedido.items.Count; e++) //pizzas
            {
                if (_pedido.items[e].subMeioMeios.Count == 0)
                {
                    _pedido.items[e].DescricaoImpressao = _pedido.items[e].NamePrincipal; // seta a impressão caso não haja complementos
                }
                else { _pedido.items[e].DescricaoImpressao = _pedido.items[e].name; }               
                

                if (_pedido.items[e].subMeioMeios != null)
                {
                    for (int e2 = 0; e2 < _pedido.items[e].subMeioMeios.Count; e2++) //metades das pizzas
                    {
                        texto = $"{_pedido.items[e].subMeioMeios[e2].name}";

                        if (_pedido.items[e].subMeioMeios[e2].complementosMeioMeio != null)
                        {

                            for (int e3 = 0; e3 < _pedido.items[e].subMeioMeios[e2].complementosMeioMeio.Count; e3++) //complementos
                            {
                                var PedidoCompl = _pedido.items[e].subMeioMeios[e2].complementosMeioMeio[e3];

                                texto += $"\n + {PedidoCompl.Quantidade} x {PedidoCompl.Complemento} R$ {PedidoCompl.Valor}"; //concatena a string de complementos
                                



                                valortotal = valortotal + (PedidoCompl.Valor * PedidoCompl.Quantidade); //total do complemento
                            }
                            _pedido.items[e].DescricaoImpressao += $"\n 1/2 {texto}";
                            lista.Add(new ComplementosDg
                            {
                                Complementos = texto //monta uma lista com as strings concatenadas para o DataGrid
                            });
                            



                        }

                        else //esconde a DataGrid caso não haja complementos
                        {
                            dgbComplementos.Visible = false;
                            dgbPedidoItens.Size = new System.Drawing.Size(Width, 668);

                        }

                    }

                    dgbComplementos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    dgbComplementos.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    dgbComplementos.Columns[0].HeaderText = "Complementos Meio/Meio";
                    dgbComplementos.Columns[0].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                    dgbComplementos.Columns[0].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);



                }


                else //esconde a DataGrid caso a pizza seja inteira
                {
                    dgbComplementos.Visible = false;
                    dgbPedidoItens.Size = new System.Drawing.Size(Width, 668);
                }





            }

            dgbComplementos.DataSource = lista; //define a DataSource com a lista de complementos




            txtVaucher.Text = _pedido.totalVoucher.ToString("C2");

            lbTipoVoucher.Text = string.Empty;
            if (_pedido.totalVoucher > 0)
                lbTipoVoucher.Text = _pedido.tipoCupom;

            txtTotalProduto.Text = _pedido.total.subTotal.ToString("C2");
            txtTotalTaxa.Text = _pedido.total.deliveryFee.ToString("C2");
            txtTotal.Text = _pedido.valorTotal.ToString("C2");

            var isPagto = _pedido.payments.methods.Where(t => t.type == "ONLINE").Any();

            lbPagamento.Text = isPagto ? "****PAGO****" : "***RECEBER NA ENTREGA****";

            // lbTroco.Text = $"Troco Para\n{_pedido.payments.methods.Sum(t => t.cash?.changeFor).Value.ToString("C2")}";
            lbTroco.Text = $"Troco Para\n{_pedido.payments.changeFor.ToString("C2")}";
            dgbPedidoItens.AutoGenerateColumns = false;


            dgbPedidoItens.DataSource = _pedido.items;
        }

        private void btnRetornar_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void btnAceitarPedido_Click(object sender, System.EventArgs e)
        {
            btnAceitarPedido.Enabled = false;

            ConfirmarPedido();

            Close();

        }
        private bool ConfirmarPedido()
        {
            try
            {
                var evento = _baseService.AlterarStatusPedido(_pedido.id, "confirm");
                //var evento = true;

                if (!evento)
                {
                    MessageBox.Show(
                        "Ocorreu um problema ao atualizar o status do pedido junto ao ZFood.\nTente novamente mais tarde.");

                    btnAceitarPedido.Enabled = true;
                    return false;
                }

                if (Program.TipoDatabase == TipoDatabaseEnum.SQLite)
                {

                    using (IPedidoGestorService pedidoGestorService = new PedidoGestorService(Program.TipoDatabase))
                    {
                        var pedidoEvento = pedidoGestorService.ObterPorPedidoId(_pedido.id);
                        _pedido.sitEvent = SituacaoVapVuptEnum.CONFIRMED.ToString();

                        pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                        pedidoEvento.Situacao = SituacaoVapVuptEnum.CONFIRMED;
                        pedidoEvento.DataHora = DateTime.Now;
                        pedidoEvento.VendaId = _pedido.id;

                        pedidoGestorService.Alterar(pedidoEvento);
                    }

                    btnImprimir.Enabled = true;
                    btnImprimir.PerformClick();

                }
                else
                {
                    foreach (var pedidoItem in _pedido.items)
                    {
                        if (pedidoItem.externalCode == "999999" && pedidoItem.options != null)
                        {
                            var itemSub = pedidoItem.options.FirstOrDefault();

                            var produtoId = itemSub != null
                                ? itemSub.externalCode
                                : pedidoItem.externalCodeCalc;

                            var desProduto = new Produto();

                            if (!string.IsNullOrEmpty(produtoId)) continue;

                            if (MessageBox.Show(
                                    "Por alguma falha no sistema (ZFood), \nnão conseguimos relacionar o produto.\nSerá necessario você fazer essa relação para continuar com o pedido.",
                                    "ZFood", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                            {
                                MessageBox.Show("Pedido não confirmado",
                                    "ZFood", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return false;
                            }
                            desProduto = RelacionarProduto.Relacionar(pedidoItem.name);
                            if (desProduto == null)
                            {
                                MessageBox.Show("Produto não relacionado\nPara continuar é necessário fazer a relação", "ZFood", MessageBoxButtons.OK);
                                return false;
                            }

                            itemSub.externalCode = desProduto.produtoId.ToString();
                        }
                    }

                    var itemRelacao = _pedido.items.Any(t => t.isRelacao == false);

                    var sitEvent = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), _pedido.sitEvent, true);
                    if (itemRelacao && sitEvent != SituacaoVapVuptEnum.INTEGRATED)
                    {
                        MessageBox.Show("Existem produtos sem relação com o E-ticket!\nVerifique os produtos e tente novamente.",
                            "Validação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }


                    using (var pedidoRepository = new PedidoRepository())
                    {
                        _pedido.senhaId = pedidoRepository.ObterSenha();

                        var vendaId = pedidoRepository.Adicionar(_pedido);


                        using (var pedidoEventoRepository = new PedidoZapFoodRepository())
                        {
                            var pedidoEvento = pedidoEventoRepository.ObterPorPedidoId(_pedido.id);
                            _pedido.sitEvent = SituacaoVapVuptEnum.CONFIRMED.ToString();
                            _pedido.vendaId = vendaId.ToString();

                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.CONFIRMED;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEvento.VendaId = vendaId.ToString();

                            pedidoEventoRepository.AddOrUpdate(pedidoEvento);


                        }

                        if (vendaId > 0)
                        {
                            btnImprimir.Enabled = true;
                            btnImprimir.PerformClick();
                        }

                    }

                }

                if (!_autoConfirma)
                {
                    MessageBox.Show("Pedido confirmado com sucesso.");
                }
                return true;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAceitarPedido.Enabled = true;
                return false;
            }
        }

        private void dgbPedidoItens_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            if (!_pedido.items[e.RowIndex].isRelacao)
            {
                var back = Color.FromArgb(211, 125, 130);
                var selecao = Color.FromArgb(184, 110, 114);
                dgbPedidoItens.Rows[e.RowIndex].DefaultCellStyle.BackColor = back;
                dgbPedidoItens.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = selecao;
            }
            else
            {
                var back = Color.FromArgb(150, 184, 110);
                var selecao = Color.FromArgb(132, 162, 97);
                dgbPedidoItens.Rows[e.RowIndex].DefaultCellStyle.BackColor = back;
                dgbPedidoItens.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = selecao;
            }
            //dgbPedidoItens.Rows[e.RowIndex].Height = 30;
            /*if (_pedido.items[e.RowIndex].addition.Count > 0)
            {
                dgbPedidoItens.Rows[e.RowIndex].Height = 42;
            }*/

        }

        private void dgbComplementos_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var back = Color.FromArgb(150, 184, 110);
            var selecao = Color.FromArgb(132, 162, 97);
            dgbComplementos.Rows[e.RowIndex].DefaultCellStyle.BackColor = back;
            dgbComplementos.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = selecao;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Realmente deseja cancelar o pedido?", "Pedido", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var sitEvent = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), _pedido.sitEvent, true);
                if (sitEvent != SituacaoVapVuptEnum.INTEGRATED) return;

                var motivo = FormMotivo.ShowFormBox("Motivo do Cancelamento.");

                var evento = _baseService.AlterarStatusPedido(_pedido.id, "requestCancellation", motivo);

                if (!evento)
                {
                    MessageBox.Show(
                        "Ocorreu um problema ao atualizar o status do pedido junto a operadora.\nTente novamente mais tarde.");

                }
                else
                {
                    if (Program.TipoDatabase == TipoDatabaseEnum.SQLite)
                    {

                        using (IPedidoGestorService pedidoGestorService = new PedidoGestorService(Program.TipoDatabase))
                        {
                            var pedidoEvento = pedidoGestorService.ObterPorPedidoId(_pedido.id);
                            _pedido.sitEvent = SituacaoVapVuptEnum.CANCELLED.ToString();

                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.CANCELLED;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEvento.VendaId = _pedido.id;

                            pedidoGestorService.Alterar(pedidoEvento);
                        }


                    }
                    else
                    {
                        using (var pedidoEventoRepository = new PedidoZapFoodRepository())
                        {

                            _pedido.sitEvent = SituacaoVapVuptEnum.CANCELLED.ToString();

                            _pedido.aplicacao = AplicacaoEnum.VapVupt;
                            var fileJson = JsonConvert.SerializeObject(_pedido);

                            var pedidoEvento = pedidoEventoRepository.ObterPorPedidoId(_pedido.id);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.CANCELLED;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEvento.FileJson = fileJson;

                            pedidoEventoRepository.AddOrUpdate(pedidoEvento);
                        }
                    }

                    MessageBox.Show("Pedido cancelado com sucesso!", "Cancelamento");
                    Close();
                }
            }

        }
        private void Entregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_pedido.isSchedule && _pedido.preparationStartDateTime > DateTime.Now)
                {
                    var result = MessageBox.Show($"Esse é um pedido agendado para {_pedido.preparationStartDateTime}\nDeseja entregar o pedido mesmo assim?",
                        "Alerta pedido agendamento.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.No)
                        return;
                }
                var evento = _baseService.AlterarStatusPedido(_pedido.id, "dispatch");
                if (!evento)
                {
                    MessageBox.Show(
                        "Ocorreu um problema ao atualizar o status do pedido junto ao ZFood.\nTente novamente mais tarde.");
                    return;
                }
                else
                {
                    if (Program.TipoDatabase == TipoDatabaseEnum.SQLite)
                    {

                        using (IPedidoGestorService pedidoGestorService = new PedidoGestorService(Program.TipoDatabase))
                        {
                            var pedidoEvento = pedidoGestorService.ObterPorPedidoId(_pedido.id);
                            _pedido.sitEvent = SituacaoVapVuptEnum.DISPATCHED.ToString();

                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.DISPATCHED;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEvento.VendaId = _pedido.id;

                            pedidoGestorService.Alterar(pedidoEvento);
                        }


                    }
                    else
                    {
                        using (IPedidoGestorService pedidoEventoRepository = new PedidoGestorService(Program.TipoDatabase))
                        {
                            var pedidoEvento = pedidoEventoRepository.ObterPorPedidoId(_pedido.id);
                            _pedido.sitEvent = SituacaoVapVuptEnum.DISPATCHED.ToString();

                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.DISPATCHED;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEventoRepository.Alterar(pedidoEvento);


                        }
                    }

                    MessageBox.Show("O status do seu pedido foi alterado com sucesso.");
                    LoadObject();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void Retorno_Click(object sender, EventArgs e)
        {
            try
            {
                var evento = _baseService.AlterarStatusPedido(_pedido.id, "delivery");
                if (!evento)
                {
                    MessageBox.Show(
                        "Ocorreu um problema ao atualizar o status do pedido junto ao ZFood.\nTente novamente mais tarde.");
                    return;
                }
                else
                {
                    if (Program.TipoDatabase == TipoDatabaseEnum.SQLite)
                    {

                        using (IPedidoGestorService pedidoGestorService = new PedidoGestorService(Program.TipoDatabase))
                        {
                            var pedidoEvento = pedidoGestorService.ObterPorPedidoId(_pedido.id);
                            _pedido.sitEvent = SituacaoVapVuptEnum.DELIVERED.ToString();

                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.DELIVERED;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEvento.VendaId = _pedido.id;

                            pedidoGestorService.Alterar(pedidoEvento);
                        }


                    }
                    else
                    {
                        using (var pedidoEventoRepository = new PedidoZapFoodRepository())
                        {
                            var pedidoEvento = pedidoEventoRepository.ObterPorPedidoId(_pedido.id);

                            _pedido.sitEvent = SituacaoVapVuptEnum.DELIVERED.ToString();
                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.DELIVERED;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEventoRepository.AddOrUpdate(pedidoEvento);

                            MessageBox.Show("O status do seu pedido foi alterado com sucesso.");
                        }
                    }

                    LoadObject();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void dgbPedidoItens_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgbPedidoItens.SelectedRows.Count == 0) return;

            var descricao = _pedido.items[e.RowIndex].name;
            var produtoRelacao = RelacionarProduto.Relacionar(descricao);
            if (produtoRelacao == null)
            {
                MessageBox.Show("Produto não relacionado\nPara continuar é necessário fazer a relação", "ZFood", MessageBoxButtons.OK);
                return;
            }

            _pedido.items[e.RowIndex].externalCode = produtoRelacao.produtoId.ToString();
            dgbPedidoItens.DataSource = null;
            LoadObject();


        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {

                if (Program.ImprimeGestor)
                {
                    var tipoOp = lbDesenvolvimento.Visible ? 2 : 1;
                    var impressao = new ImprimirComprovante(_pedido);
                    impressao.Imprimir(tipoOp);
                    lbDesenvolvimento.Visible = false;
                    return;
                }

                using (var pedidoRepository = new PedidoRepository())
                {

                    var tipo = _pedido.orderType == "DELIVERY" ? 5 : _pedido.orderType == "TAKEOUT" ? 6 : 7;

                    using (var pedidoEventoRepository = new PedidoZapFoodRepository())
                    {
                        var pedidoVenda = pedidoEventoRepository.ObterPorPedidoId(_pedido.id);

                        if (pedidoVenda == null) return;
                        pedidoRepository.ImprimirFechamentoPedido(int.Parse(pedidoVenda.VendaId), tipo);

                        //MessageBox.Show("Im", "ZFood", MessageBoxButtons.OK);
                    }

                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "ZFood", MessageBoxButtons.OK);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                var evento = _baseService.AlterarStatusPedido(_pedido.id, "readyToPickup");
                if (!evento)
                {
                    MessageBox.Show(
                        "Ocorreu um problema ao atualizar o status do pedido junto ao ZFood.\nTente novamente mais tarde.");
                    return;
                }
                else
                {
                    if (Program.TipoDatabase == TipoDatabaseEnum.SQLite)
                    {

                        using (IPedidoGestorService pedidoGestorService = new PedidoGestorService(Program.TipoDatabase))
                        {
                            var pedidoEvento = pedidoGestorService.ObterPorPedidoId(_pedido.id);
                            _pedido.sitEvent = SituacaoVapVuptEnum.READYTODELIVER.ToString();

                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.READYTODELIVER;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEvento.VendaId = _pedido.id;

                            pedidoGestorService.Alterar(pedidoEvento);
                        }


                    }
                    else
                    {
                        using (var pedidoEventoRepository = new PedidoZapFoodRepository())
                        {
                            var pedidoEvento = pedidoEventoRepository.ObterPorPedidoId(_pedido.id);
                            _pedido.sitEvent = SituacaoVapVuptEnum.READYTODELIVER.ToString();

                            pedidoEvento.FileJson = JsonConvert.SerializeObject(_pedido);
                            pedidoEvento.Situacao = SituacaoVapVuptEnum.READYTODELIVER;
                            pedidoEvento.DataHora = DateTime.Now;
                            pedidoEventoRepository.AddOrUpdate(pedidoEvento);

                            MessageBox.Show("O status do seu pedido foi alterado com sucesso.");
                        }
                    }
                    LoadObject();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void FormPedidoItens_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
                lbDesenvolvimento.Visible = true;
        }

        private void txtNPedido_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var pedido = _baseService.ObterById(_pedido.id);
        }
    }
}
