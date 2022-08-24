using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ZapFood.WinForm.Componente.PopupNotifier;
using ZapFood.WinForm.Layout;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Properties;
using ZapFood.WinForm.Service;
using System.Media;
using Newtonsoft.Json;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Data.Service;
using ZapFood.WinForm.Data.Service.Interface;
using ZapFood.WinForm.Wizard;
using System.Runtime.InteropServices;

//teste feito em: http://zclub.com.br:8989/cantinagrill/

namespace ZapFood.WinForm
{
    public partial class FormPrincipal : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private List<PedidoRootModel> _pedidos;
        private List<Model.DeliveryApp.order> _deliveryAppOrders { get; set; }
        private readonly IPedidoGestorService _pedidoRepository;
        private readonly PedidoService _pedidoService;
        private readonly IBaseService _pedidoIfoodService;
        private readonly IBaseService _pedidoVapVuptService;
        private bool isNewDeliveryIfood;
        private bool isNewDeliveryIVap;
        private DateTime _dtRefreshStatus;
        private DateTime _dtStatusIfood;
        private DateTime _dtStatusZapp;
        private bool _errorIfood = false;
        private bool _errorZapp = false;
        private bool _errorDeliveryApp = false;        
        private LogWriter _logWriter;
        private string _deliveryAppToken;
        private List<RestauranteShifts> _restauranteShiftses;
        private readonly RestauranteShiftService _restauranteShiftService;
        private readonly ComplementosService _complementosService;

        public FormPrincipal()
        {
            InitializeComponent();           

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            pnlNav.Height = btnPedidoNovos.Height;
            pnlNav.Top = btnPedidoNovos.Top;
            pnlNav.Left = btnPedidoNovos.Left;

            _pedidoRepository = new PedidoGestorService(Program.TipoDatabase);
            _pedidoService = new PedidoService();
            _pedidoIfoodService = new PedidoIfoodService();
            _pedidoVapVuptService = new PedidoVapVuptService();
            _logWriter = new LogWriter();
            //Desabilita itens não ultilizado quando app proprio  não é ultilizado
            toolStripButton2.Visible = !string.IsNullOrEmpty(Program.TokenVapVupt);
            toolStripLabel1.Visible = !string.IsNullOrEmpty(Program.TokenVapVupt);
            toolStripSplitButton1.Visible = !string.IsNullOrEmpty(Program.TokenVapVupt);



            tablessControl1.Selected += tabControl1_Selected;

            CheckForIllegalCrossThreadCalls = false;
        }

        private string GetImagem()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    pictureBox1.BackgroundImage.Save(stream, ImageFormat.Png);
                    stream.Close();
                    byte[] imageBytes = stream.ToArray();
                    var base64String = Convert.ToBase64String(imageBytes);
                    return base64String;

                }
            }
            catch (Exception e)
            {
                return String.Empty;

            }

        }

        void CarregaLogo()
        {
            var logo = $"{AppDomain.CurrentDomain.BaseDirectory}Logo.Png";
            if (File.Exists(logo))
                pictureBox1.Image = Image.FromFile(logo);

            else if (Program.Restaurante == null || Program.Restaurante.Imagem == null)
            {
                pictureBox1.Image = null;
            } 
            else
            {          
            
                try
                {
                    byte[] imageBytes = Convert.FromBase64String(Program.Restaurante.Imagem.Replace("data:image/png;base64,", "").Replace("data:image/jpeg;base64,", ""));
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                catch
                {
                    //Ignore
                }

            }
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            _pedidos = new List<PedidoRootModel>();

            tsbStatusIfood.Visible = false;
            tsbStatusZapp.Visible = false;


            if (!string.IsNullOrEmpty(Program.MerchantId))
            {
                using (var merchantService = new MerchantService())
                {
                    Program.Merchant = merchantService.ObterLoja().Result;

                    if (Program.Merchant != null)
                    {
                        lbLojaNome.Text = Program.Merchant.name;
                    }


                    tsbStatusIfood.Text = "Loja Fechada";
                    tsbStatusIfood.ToolTipText = "Gestor de Pedidos fechado";
                    tsbStatusIfood.BackColor = Color.Red;
                    tsbStatusIfood.Image = Resources.circle_red_16;
                }

                timerPrincipal.Interval = 100;
                timerPrincipal.Tick += timerPrincipal_Tick;
                timerPrincipal.Start();

                tsbStatusIfood.Visible = true;


            }

            //Inicia delivery App (Neemo)
            if (!string.IsNullOrEmpty(Program.TokenDeliveryApp))
            {
                _deliveryAppToken = Program.TokenDeliveryApp;
                _deliveryAppOrders = new List<Model.DeliveryApp.order>();

                timerDeliveryApp.Interval = 100;
                timerDeliveryApp.Tick += timerDeliveryApp_Tick;
                timerDeliveryApp.Start();
            }


            if (!string.IsNullOrEmpty(Program.TokenVapVupt))
            {
                timerZFood.Interval = 100;
                timerZFood.Start();

                tsbStatusZapp.Visible = true;
            }

            timerControles.Interval = 100;
            timerControles.Start();

            timerEntregas.Interval = 30000;
            timerEntregas.Start();

            var restauranteService = new ComplementosService();
            var restaurante = restauranteService.ObterComplementosMeioMeio();


            //btnPedidoNovos.PerformClick();
            tabControl1_Selected(tabPedidoNovos, new TabControlEventArgs(tabPedidoNovos, 0, TabControlAction.Selected));

            CarregaLogo();
        }

        private void timerZFood_Tick(object sender, EventArgs e)
        {
            try
            {
                bwZFood.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                _errorZapp = true;
                _logWriter.LogWrite(exception.Message);
            }

        }
        private void timerControles_Tick(object sender, EventArgs e)
        {
            try
            {
                timerControles.Stop();
                CarregaControles();

                lbStatusLoja.Text = $"ATUALIZADO: {DateTime.Now.ToString("HH:mm:ss")}";


            }
            catch (Exception exception)
            {
                _logWriter.LogWrite(exception.Message);
            }
            finally
            {
                timerControles.Interval = 25000;
                timerControles.Start();
            }

        }

        private void timerEntregas_Tick(object sender, EventArgs e)
        {
            try
            {
                timerEntregas.Stop();

                AtualizaEntregas();
            }
            catch (Exception exception)
            {
                _logWriter.LogWrite(exception.Message);
            }
            finally
            {
                timerEntregas.Interval = 80000;
                timerEntregas.Start();
            }
        }

        private void bwZFood_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _dtStatusZapp = DateTime.Now;
                _errorZapp = false;

                timerZFood.Stop();

                PedidosVapVupt();

                CarregaSituacaoLoja();

            }
            catch (Exception exception)
            {
                _errorZapp = true;
                _logWriter.LogWrite(exception.Message);
            }
            finally
            {

            }
        }

        private void bwZFood_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_errorZapp)
            {
                tsbStatusZapp.Image = Resources.circle_red_16;
                timerZFood.Interval = 5000;
            }

            else
            {
                var tempo = DateTime.Now.Second - _dtStatusZapp.Second;

                if (tempo <= 7)
                    tsbStatusZapp.Image = Resources.circle_green_16;
                else if (tempo <= 20)
                    tsbStatusZapp.Image = Resources.circle_yellow_16;
                else
                    tsbStatusZapp.Image = Resources.circle_red_16;

                timerZFood.Interval = 25000;
            }
            timerZFood.Start();
        }

        private void bwIFood_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _dtStatusIfood = DateTime.Now;
                _errorIfood = false;

                timerPrincipal.Stop();

                PedidoIfood();

                VerificaStatusLoja();

            }
            catch (Exception exception)
            {
                _errorIfood = true;
                _logWriter.LogWrite(exception.Message);
            }
            finally
            {

            }
        }
        private void bwIFood_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_errorIfood)
            {
                //tsbStatusIfood.Image = Resources.circle_red_16;
                timerPrincipal.Interval = 5000;
            }

            else
            {
                var tempo = DateTime.Now.Second - _dtStatusIfood.Second;

                /* if (tempo <= 10)
                     tsbStatusIfood.Image = Resources.circle_green_16;
                 else if (tempo <= 20)
                     tsbStatusIfood.Image = Resources.circle_yellow_16;
                 else
                     tsbStatusIfood.Image = Resources.circle_red_16;
                */
                timerPrincipal.Interval = 25000;
            }


            timerPrincipal.Start();
        }
        void timerPrincipal_Tick(object sender, EventArgs e)
        {

            try
            {
                bwIFood.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                _errorIfood = true;
                _logWriter.LogWrite(exception.Message);
            }

        }

        void VerificaStatusLoja()
        {
            using (var lojaService = new MerchantService())
            {
                if (Program.Merchant == null)
                {
                    using (var merchantService = new MerchantService())
                    {
                        Program.Merchant = merchantService.ObterLoja().Result;
                    }
                    }
                    
                var status = lojaService.StatusLoja();
                if (status != null)
                {

                    switch (status.state)
                    {
                        case "OK":
                            tsbStatusIfood.Text = $"{status.message.title}";
                            tsbStatusIfood.ToolTipText = status.message.subtitle;
                            tsbStatusIfood.BackColor = Color.Green;
                            tsbStatusIfood.Image = Resources.circle_green_16;
                            break;
                        case "WARNING":
                            tsbStatusIfood.Text = $"{status.message.title}";
                            tsbStatusIfood.ToolTipText = status.message.subtitle;
                            tsbStatusIfood.BackColor = Color.Yellow;
                            tsbStatusIfood.Image = Resources.circle_yellow_16;
                            break;

                        case "CLOSED":
                            tsbStatusIfood.Text = $"{status.message.title}";
                            tsbStatusIfood.ToolTipText = status.message.subtitle;
                            tsbStatusIfood.BackColor = Color.Gray;
                            tsbStatusIfood.Image = Resources.verify_correct_icone_9268_16;
                            break;
                        case "ERROR":
                            tsbStatusIfood.Text = $"{status.message.title}";
                            tsbStatusIfood.ToolTipText = status.message.subtitle;
                            tsbStatusIfood.BackColor = Color.Red;
                            tsbStatusIfood.Image = Resources.circle_red_16;
                            break;
                        default:
                            break;
                    }


                }
                else
                {
                    tsbStatusIfood.Text = "Loja fechada verifique sua internet.";
                    tsbStatusIfood.BackColor = Color.Red;
                    tsbStatusIfood.Image = Resources.circle_red_16;
                }


            }

        }

        void timerDeliveryApp_Tick(object sender, EventArgs e)
        {

            try
            {
                bwDeliveryApp.RunWorkerAsync();
            }
            catch (Exception exception)
            {
                _errorDeliveryApp = true;
                _logWriter.LogWrite(exception.Message);
            }


        }
        private void bwDeliveryApp_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _dtStatusIfood = DateTime.Now;
                _errorDeliveryApp = false;

                timerDeliveryApp.Stop();

                deliveryApp();

     

            }
            catch (Exception exception)
            {
                _errorIfood = true;
                _logWriter.LogWrite(exception.Message);
            }
            finally
            {

            }
        }
        private void bwDeliveryApp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_errorDeliveryApp)
            {
                //tsbStatusIfood.Image = Resources.circle_red_16;
                timerDeliveryApp.Interval = 5000;
            }

            else
            {
                var tempo = DateTime.Now.Second - _dtStatusIfood.Second;

                /* if (tempo <= 10)
                     tsbStatusIfood.Image = Resources.circle_green_16;
                 else if (tempo <= 20)
                     tsbStatusIfood.Image = Resources.circle_yellow_16;
                 else
                     tsbStatusIfood.Image = Resources.circle_red_16;
                */
                timerDeliveryApp.Interval = 25000;
            }


            timerDeliveryApp.Start();
        }

        private void deliveryApp()
        {
            var deliveryAppService = new PedidoDeliveyAppService();
            
            try
            {
                var orderResult = deliveryAppService.Orders(_deliveryAppToken, (byte)DeliveryApp.Enum.OrderStatus.NovoPedido);
                if (orderResult.Success)
                {
                    foreach (var result in orderResult.Result)
                    {
                        var order = deliveryAppService.Order(_deliveryAppToken, result.id.ToString());

                        if (!order.Success) return;
                        if (order.Result.status != 0) return;

                        var pedido = DeliveryApp.Utils.ConvertApp.PedidoParse(order.Result);

                        
                        pedido.aplicacao = AplicacaoEnum.Neemo;
                        
                        var fileJson = JsonConvert.SerializeObject(pedido);
                        var pedidoZapFood = new PedidoVapVupt()
                        {
                            VapVuptId = Guid.NewGuid(),
                            PedidoId = pedido.id,
                            DataHora = DateTime.Now,
                            Situacao = SituacaoVapVuptEnum.INTEGRATED,
                            Aplicacao = AplicacaoEnum.Neemo,
                            FileJson = fileJson,
                            TipoPedido = pedido.orderType == "DELIVERY" ? "Entregar" : pedido.orderType == "TAKEOUT" ? "Retirar" : "Mesa"
                        };

                        _pedidoRepository.Adicionar(pedidoZapFood);
                    }
                    _deliveryAppOrders.AddRange(orderResult.Result);


                }
                else
                {
                    MessageBox.Show(orderResult.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                    message = ex.InnerException.Message;

                MessageBox.Show(message);
            }
        }

        private void PedidosVapVupt()
        {
            try
            {
              var pollings = _pedidoVapVuptService.Obterpolling();


                foreach (var pollingModel in pollings)
                {
                    var situacao = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), pollingModel.code, true);
                    if (situacao != SituacaoVapVuptEnum.PLACED && situacao != SituacaoVapVuptEnum.CANCELLED
                        && situacao != SituacaoVapVuptEnum.CONFIRMED) continue;

                    PedidoRootModel pedido;

                    try
                    {
                        pedido = _pedidoVapVuptService.ObterById(pollingModel.orderId);
                    }
                    catch (Exception e)
                    {
                        ReadError(e.Message);
                        continue;
                    }




                    if (pedido == null) continue;

                    if (Program.Monitorar.All(t => t != pedido.orderType.Trim().ToUpper()))
                        continue;

                    pedido.sitEvent = situacao == SituacaoVapVuptEnum.PLACED
                        ? SituacaoVapVuptEnum.INTEGRATED.ToString()
                        : situacao != SituacaoVapVuptEnum.CONFIRMED ? SituacaoVapVuptEnum.CONFIRMED.ToString() : SituacaoVapVuptEnum.CANCELLED.ToString();

                    pedido.aplicacao = AplicacaoEnum.VapVupt;

                    var fileJson = JsonConvert.SerializeObject(pedido);
                    var pedidoZapFood = new PedidoVapVupt()
                    {
                        VapVuptId = Guid.NewGuid(),
                        PedidoId = pollingModel.orderId,
                        DataHora = DateTime.Now,
                        Situacao = situacao,
                        Aplicacao = AplicacaoEnum.VapVupt,
                        FileJson = fileJson,
                        TipoPedido = pedido.orderType == "DELIVERY" ? "Entregar" : pedido.orderType == "TAKEOUT" ? "Retirar" : "Mesa"
                    };

                    _pedidoRepository.Adicionar(pedidoZapFood);

                    if (situacao == SituacaoVapVuptEnum.PLACED)
                    {
                        _pedidoVapVuptService.AlterarStatusPedido(pollingModel.orderId, "integration");
                        // btnPedidoNovos.PerformClick();
                    }

                    if (situacao == SituacaoVapVuptEnum.CANCELLED)
                    {
                        _pedidoVapVuptService.AlterarStatusPedido(pollingModel.orderId, "cancelled");
                        //btnCancelados.PerformClick();
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Função: PedidosVapVupt MSG: {e.Message}");
            }
        }

        private void ReadError(object error)
        {
            if (error is Exception)
            {
                MessageBox.Show((error as Exception).Message);
            }
        }
        /*
        private void AtualizarPedidos()
        {

            try
            {
                if (Program.CaixaId == 0)
                {
                    MessageBox.Show(
                        "Não é possivel consultar pedidos com o Caixa Fechado!\n Abra o caixa que seus pedios serão exibidos.");
                    return;
                }
            }
            catch (Exception e)
            {
                ReadErrorGeral(new Exception("Erro na conexão com o servidor!"));
                return;
            }
            try
            {
                timerPrincipal.Stop();

                _pedidos = new List<PedidoRootModel>();



                

                //Carrega Contoles na tela

                CarregaControles();

                lbStatusLoja.Text = $"ATUALIZADO: {DateTime.Now.ToString("HH:mm:ss")}";
            }
            catch (Exception e)
            {
                string result = MyMessageBox.ShowBox(
                    $"Estamos com dificuldade em acessar o servidor!\nVerifique se sua internet esta funcionando e tente novamente.\n{e.Message}",
                    "Falha de conexão");

                if (result.Equals("1"))
                {
                    AtualizarPedidos();
                }
            }
            finally
            {
                timerPrincipal.Start();
            }
        }
        */
        private void CarregaSituacaoLoja()
        {
            if (string.IsNullOrEmpty(Program.TokenVapVupt))
            {
                toolStripLabel1.Visible = false;
                return;
            }


            var status = Program.Restaurante.Situacao == 3 ? "Fechado pelo restaurante" : "Aberto";
            toolStripLabel1.Text = $"{Program.Restaurante.Nome} ({status})";
            toolStripLabel1.Image = Program.Restaurante.Situacao == 3
                ? Resources.circle_red_16
                : Resources.circle_green_16;
        }

        private void PedidoIfood()
        {
            try
            {
                var pollings = _pedidoIfoodService.Obterpolling();
                var pollingsOk = new List<Model.Ifood.PollingModel>();
                foreach (var pollingModel in pollings)
                {
                    try
                    {

                        //if (situacao != SituacaoVapVuptEnum.PLACED && situacao != SituacaoVapVuptEnum.CANCELLED) continue;

                        var situacao = pollingModel.situacao;

                        if (situacao != SituacaoVapVuptEnum.PLACED && situacao != SituacaoVapVuptEnum.CANCELLED && situacao != SituacaoVapVuptEnum.INTEGRATED)
                        {
                            var pedidoZap = _pedidoRepository.ObterById(Guid.Parse(pollingModel.orderId));
                            if (pedidoZap != null)
                            {
                                var pedidoAlter = JsonConvert.DeserializeObject<PedidoRootModel>(pedidoZap.FileJson);
                                pedidoAlter.sitEvent = situacao.ToString();
                                pedidoAlter.aplicacao = AplicacaoEnum.Ifood;
                                pedidoZap.FileJson = JsonConvert.SerializeObject(pedidoAlter);
                                pedidoZap.Situacao = situacao;
                                _pedidoRepository.Adicionar(pedidoZap);
                                pollingsOk.Add(pollingModel);
                                continue;
                            }
                        }

                        var pedido = _pedidoIfoodService.ObterById(pollingModel.orderId);
                        if (pedido == null) continue;

                        if (Program.TipoAplicativoIfood == Model.Ifood.TipoAplicativoEnumModel.Distribuído)
                            if (pedido.merchant.id != Program.MerchantId) continue;


                        pedido.sitEvent = situacao.ToString();
                        pedido.aplicacao = AplicacaoEnum.Ifood;

                        var fileJson = JsonConvert.SerializeObject(pedido);
                        var pedidoZapFood = new PedidoVapVupt()
                        {
                            VapVuptId = Guid.NewGuid(),
                            PedidoId = pollingModel.orderId,
                            DataHora = DateTime.Now,
                            Situacao = situacao,
                            Aplicacao = AplicacaoEnum.Ifood,
                            FileJson = fileJson,
                            TipoPedido = pedido.orderType == "DELIVERY" ? "Entregar" : pedido.orderType == "TAKEOUT" ? "Retirar" : "Mesa"
                        };

                        _pedidoRepository.Adicionar(pedidoZapFood);
                        pollingsOk.Add(pollingModel);

                        if (situacao == SituacaoVapVuptEnum.PLACED)
                        {
                            //_pedidoIfoodService.PollingAcknowledgment(pollingModel);
                            //btnPedidoNovos.PerformClick();
                        }

                        if (situacao == SituacaoVapVuptEnum.CANCELLED)
                        {
                            _pedidoIfoodService.AlterarStatusPedido(pollingModel.orderId, "acceptCancellation");
                            //btnCancelados.PerformClick();
                        }

                    }
                    catch (Exception e)
                    {
                       
                        throw new Exception($"Função: PollingIfood MSG: {e.Message}");

                    }

                }
                if (pollings.Count > 0)
                {
                    _pedidoIfoodService.PollingAcknowledgment(pollingsOk);
                }
            }
            catch (Exception e)
            {
                Program.GetToken(true);
                throw new Exception($"Função: PollingGeralIfood MSG: {e.Message}");
            }
        }

        private void CarregaPedidoNovos()
        {
            flowLayoutPanel1.Controls.Clear();
            var eventodePedidos = _pedidoRepository.ObterNovos();
            foreach (var eventodePedido in eventodePedidos)
            {
                try
                {
                    if (string.IsNullOrEmpty(eventodePedido.FileJson)) continue;
                    var pedido = JsonConvert.DeserializeObject<PedidoRootModel>(eventodePedido.FileJson);


                    if (pedido.sitEvent == null)
                        pedido.sitEvent = eventodePedido.Situacao.ToString();


                    var sitEvent = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), pedido.sitEvent, true);
                    if (sitEvent != SituacaoVapVuptEnum.INTEGRATED && sitEvent != SituacaoVapVuptEnum.PLACED) continue;

                    if (Program.AutoConfirmar)
                    {
                        try
                        {
                            using (var form = new FormPedidoItens(pedido, true))
                            {
                                timerZFood.Stop();

                                form.Visible = false;
                                form.ShowDialog();

                                timerZFood.Start();
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logWriter.LogWrite($"Função AutoConfirma Erro: {ex.Message}");
                            timerZFood.Start();
                            continue;
                        }
                        finally
                        {

                        }


                    }

                    var row = new RowPedido();
                    row.Cliente = $"{pedido.customer?.name}";
                    row.PedidoId = pedido.displayId;
                    row.DataHora = pedido.DataPedido.ToString("dd-MM-yyyy HH:mm");
                    row.DataAgendamento = pedido.isSchedule ? pedido.schedule?.deliveryDateTimeStart.ToString("dd-MM-yyyy HH:mm") : "";
                    row.Endereco = pedido.delivery != null
                        ? $"{pedido.delivery.deliveryAddress?.formattedAddress}  {pedido.delivery.deliveryAddress?.neighborhood}"
                        : "RETIRAR";

                    //correção temporaria na row endereço até o "orderType" estar corrigido no app de pedidos
                    if (pedido.orderType != "DELIVERY" && pedido.orderType != "INDOOR") row.Endereco = "RETIRAR";

                        

                    row.Telefone = pedido.customer?.phone.number;
                    row.Tipo = pedido.orderType;
                    row.Status = pedido.sitEvent;
                    row.Aplicacao = pedido.aplicacao;

                    row.ItemSource = pedido;

                    row.ButtonText = "Detalhes do Pedido";
                    row.ConfirmEvent += Row_Click;
                    flowLayoutPanel1.Controls.Add(row);
                }
                catch (Exception e)
                {
                    throw new Exception($"Função: CarregaPedidoNovos MSG: {e.Message}");
                }

            }
            lbQtdePedidosNovos.Text = flowLayoutPanel1.Controls.Count.ToString();

            /*if (flowLayoutPanel1.Controls.Count > 0)
            {
                
                btnPedidoNovos.PerformClick();
                
            }*/


        }

        private void CarregaPedidosPendenteEntrega()
        {
            var eventodePedidos = _pedidoRepository.ObterPorSituacao(SituacaoVapVuptEnum.CONFIRMED);

            flowLayoutPanel2.Controls.Clear();

            foreach (var eventodePedido in eventodePedidos)
            {
                try
                {
                    if (string.IsNullOrEmpty(eventodePedido.FileJson)) continue;
            
                    var pedido = JsonConvert.DeserializeObject<PedidoRootModel>(eventodePedido.FileJson);


                    if (pedido.sitEvent == null)
                        pedido.sitEvent = eventodePedido.Situacao.ToString();


                    var sitEvent = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), pedido.sitEvent, true);
                    if (sitEvent != SituacaoVapVuptEnum.CONFIRMED) continue;


                    var row = new RowPedido();
                    row.Cliente = $"{pedido.customer?.name}";
                    row.PedidoId = pedido.displayId;
                    row.DataHora = pedido.DataPedido.ToString("dd-MM-yyyy HH:mm");
                    row.DataAgendamento = pedido.isSchedule ? pedido.schedule?.deliveryDateTimeStart.ToString("dd-MM-yyyy HH:mm") : "";
                    row.Endereco = pedido.delivery != null
                        ? $"{pedido.delivery.deliveryAddress?.formattedAddress} {pedido.delivery.deliveryAddress?.streetNumber} {pedido.delivery.deliveryAddress?.neighborhood}"
                        : "RETIRAR";

                    row.Telefone = pedido.customer?.phone.number;
                    row.Tipo = pedido.orderType;
                    row.Status = pedido.sitEvent;
                    row.Aplicacao = pedido.aplicacao;

                    row.ItemSource = pedido;

                    row.ButtonText = "Detalhes do Pedido";
                    row.ConfirmEvent += RowEntrega_Click;
                    flowLayoutPanel2.Controls.Add(row);
                }
                catch (Exception e)
                {
                    _logWriter.LogWrite($"Função: CarregaPedidosPendenteEntrega MSG: {e.Message}");
                    continue;
                }

            }
        }

        private void CarregaPedidosPendenteRetorno()
        {
            try
            {
                var eventodePedidos = _pedidoRepository.ObterPorSituacao(SituacaoVapVuptEnum.DISPATCHED);

                flowLayoutPanel3.Controls.Clear();

                foreach (var eventodePedido in eventodePedidos)
                {
                    if (string.IsNullOrEmpty(eventodePedido.FileJson)) continue;
                    var pedido = JsonConvert.DeserializeObject<PedidoRootModel>(eventodePedido.FileJson);


                    if (pedido.sitEvent == null)
                        pedido.sitEvent = eventodePedido.Situacao.ToString();



                    var sitEvent = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), pedido.sitEvent, true);
                    if (sitEvent != SituacaoVapVuptEnum.DISPATCHED) continue;


                    var row = new RowPedido();
                    row.Cliente = $"{pedido.customer?.name}";
                    row.PedidoId = pedido.displayId;
                    row.DataHora = pedido.DataPedido.ToString("dd-MM-yyyy HH:mm");
                    row.DataAgendamento = pedido.isSchedule ? pedido.schedule?.deliveryDateTimeStart.ToString("dd-MM-yyyy HH:mm") : "";
                    row.Endereco = pedido.delivery != null
                        ? $"{pedido.delivery.deliveryAddress?.formattedAddress} {pedido.delivery.deliveryAddress?.streetNumber} {pedido.delivery.deliveryAddress?.neighborhood}"
                        : "RETIRAR";

                    row.Telefone = pedido.customer?.phone.number;
                    row.Tipo = pedido.orderType;
                    row.Status = pedido.sitEvent;
                    row.Aplicacao = pedido.aplicacao;

                    row.ItemSource = pedido;
                    row.ButtonText = "Detalhes do Pedido";
                    row.ConfirmEvent += Row_Click;
                    flowLayoutPanel3.Controls.Add(row);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Função: CarregaPedidosPendenteRetorno MSG: {e.Message}");

            }

        }

        private void CarregaPedidosCancelados()
        {
            try
            {
                var eventodePedidos = _pedidoRepository.ObterPorSituacao(SituacaoVapVuptEnum.CANCELLED);

                flowLayoutPanel5.Controls.Clear();

                foreach (var eventodePedido in eventodePedidos)
                {
                    if (string.IsNullOrEmpty(eventodePedido.FileJson)) continue;
                    var pedido = JsonConvert.DeserializeObject<PedidoRootModel>(eventodePedido.FileJson);


                    if (pedido.sitEvent == null)
                        pedido.sitEvent = eventodePedido.Situacao.ToString();



                    var sitEvent = (SituacaoVapVuptEnum)Enum.Parse(typeof(SituacaoVapVuptEnum), pedido.sitEvent, true);
                    if (sitEvent != SituacaoVapVuptEnum.CANCELLED) continue;

                    var row = new RowPedido();
                    row.Cliente = $"{pedido.customer?.name}";
                    row.PedidoId = pedido.shortReference;
                    row.DataHora = pedido.DataPedido.ToString("dd-MM-yyyy HH:mm");
                    row.DataAgendamento = pedido.isSchedule ? pedido.schedule?.deliveryDateTimeStart.ToString("dd-MM-yyyy HH:mm") : "";
                    row.Endereco = pedido.delivery != null
                        ? $"{pedido.delivery.deliveryAddress?.formattedAddress} {pedido.delivery.deliveryAddress?.streetNumber} {pedido.delivery.deliveryAddress?.neighborhood}"
                        : "RETIRAR";

                    row.Telefone = pedido.customer?.phone.number;
                    row.Tipo = pedido.orderType;
                    row.Status = pedido.sitEvent;
                    row.Aplicacao = pedido.aplicacao;

                    row.ItemSource = pedido;
                    row.ButtonText = "Detalhes do Pedido";
                    row.ConfirmEvent += Row_Click;
                    flowLayoutPanel5.Controls.Add(row);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Função: CarregaPedidosCancelados MSG: {e.Message}");

            }

        }

        private void CarregaControles()
        {
            try
            {
                CarregaPedidoNovos();


                AlertPopUp(_pedidos);


                lbStatusLoja.Text = "Loja Aberta";
                lbStatusLoja.ForeColor = Color.DarkGreen;
                panel1.BackColor = Color.DarkGray;


            }
            catch (Exception e)
            {
                throw new Exception($"Função: CarregaPedidoNovos MSG: {e.Message}");
            }

        }

        private void Row_Click(object sender, EventArgs e)
        {
            var item = (RowPedido)sender;
            using (var form = new FormPedidoItens((PedidoRootModel)item.ItemSource, false))
            {
                form.ShowDialog();

                CarregaControles();

                //tabControl1_Selected(sender, null);
            }
        }
        private void RowEntrega_Click(object sender, EventArgs e)
        {
            var item = (RowPedido)sender;
            using (var form = new FormPedidoItens((PedidoRootModel)item.ItemSource, false))
            {
                form.ShowDialog();

                CarregaPedidosPendenteEntrega();

                //tabControl1_Selected(sender, null);
            }
        }

        private void AtualizaEntregas()
        {
            try
            {
                if (Program.TipoDatabase == TipoDatabaseEnum.SQLite)
                    return;


                //if (_dtRefreshStatus.AddMinutes(2) >= DateTime.Now) return;

                IBaseService service;

                var pedidoEventos = _pedidoRepository.ObterPedidosEntregando();
                foreach (var pedidoVapVupt in pedidoEventos)
                {
                    var pedido = pedidoVapVupt.PedidoRootModel;

                    switch (pedido.aplicacao)
                    {
                        case AplicacaoEnum.Ifood:
                            service = new PedidoIfoodService();
                            break;
                        case AplicacaoEnum.VapVupt:
                            service = new PedidoVapVuptService();
                            break;
                        case AplicacaoEnum.Neemo:
                            service = new PedidoDeliveyAppService();
                            break;
                        default:
                            service = new PedidoIfoodService();
                            break;
                    }

                    try
                    {
                        var eventoEntregue = service.AlterarStatusPedido(pedidoVapVupt.PedidoId, "dispatch");
                        if (!eventoEntregue) continue;
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Erro na função AlteraStatusdelivery MSG: {e.Message}");
                    }


                    var pedidoEvento = _pedidoRepository.ObterPorPedidoId(pedidoVapVupt.PedidoId);
                    pedido.sitEvent = SituacaoVapVuptEnum.DISPATCHED.ToString();

                    pedidoEvento.FileJson = JsonConvert.SerializeObject(pedido);

                    pedidoEvento.Situacao = SituacaoVapVuptEnum.DISPATCHED;
                    pedidoEvento.DataHora = DateTime.Now;
                    _pedidoRepository.Adicionar(pedidoEvento);
                }
                var pedidoEventoEntrege = _pedidoRepository.ObterPedidosEntreges();
                foreach (var pedidoVapVupt in pedidoEventoEntrege)
                {
                    var pedido = pedidoVapVupt.PedidoRootModel;
                    if (pedido.aplicacao == AplicacaoEnum.VapVupt)
                        service = new PedidoVapVuptService();
                    else
                        service = new PedidoIfoodService();

                    try
                    {
                        service.AlterarStatusPedido(pedidoVapVupt.PedidoId, "delivery");
                    }
                    catch (Exception ex)
                    {

                        throw new Exception($"Erro na função AlteraStatusdelivery MSG: {ex.Message}");
                    }

                    var pedidoEvento = _pedidoRepository.ObterPorPedidoId(pedidoVapVupt.PedidoId);
                    pedido.sitEvent = SituacaoVapVuptEnum.DELIVERED.ToString();
                    pedidoEvento.Situacao = SituacaoVapVuptEnum.DELIVERED;
                    pedidoEvento.DataHora = DateTime.Now;
                    _pedidoRepository.Adicionar(pedidoEvento);
                }
                try
                {
                    var pedidosRetirados = _pedidoRepository.ObterPedidosRetirar();
                    foreach (var pedidoVapVupt in pedidosRetirados)
                    {
                        var pedido = pedidoVapVupt.PedidoRootModel;
                        if (pedido.aplicacao == AplicacaoEnum.VapVupt)
                            service = new PedidoVapVuptService();
                        else
                            service = new PedidoIfoodService();

                        try
                        {
                            service.AlterarStatusPedido(pedidoVapVupt.PedidoId, "readyToDeliver");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Erro na função: AlterarStatus(readyToDeliver) MSG: {ex.Message}");
                        }

                        var pedidoEvento = _pedidoRepository.ObterPorPedidoId(pedidoVapVupt.PedidoId);
                        pedido.sitEvent = SituacaoVapVuptEnum.DELIVERED.ToString();
                        pedidoEvento.Situacao = SituacaoVapVuptEnum.DELIVERED;
                        pedidoEvento.DataHora = DateTime.Now;
                        _pedidoRepository.Adicionar(pedidoEvento);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Função: CarregaPedidoNovos MSG: {e.Message}");

                }
                _dtRefreshStatus = DateTime.Now;
            }
            catch
            {
                ReadErrorGeral(new Exception("Encontramos um problema com a conexão ao servidor!"));
                /*MessageBox.Show(
                    "Encontramos um problema com a conexão ao servidor!\nVerifique sua rede ou entre em contato com seu suporte Zip!",
                    "Falha geral");*/
            }

        }

        void AlertPopUp(List<PedidoRootModel> pedidos)
        {
            if (flowLayoutPanel1.Controls.Count == 0) return;
            //if (pedidos.Where(t => t.sitEvent == "INTEGRATED").ToList().Count == 0) return;

            popupNotifier1 = new PopupNotifier
            {
                TitleText = "Alerta de pedidos Zip Food.",
                ContentText = $"Olá existe {flowLayoutPanel1.Controls.Count} pedido(s) para confirmação.",

                ShowCloseButton = true,
                ShowOptionsButton = true,
                ShowGrip = true,
                Delay = 9000,
                AnimationInterval = 10,
                AnimationDuration = 200,
                TitlePadding = new Padding(0),
                ContentPadding = new Padding(0),
                ImagePadding = new Padding(0),
                Scroll = true,
                Image = Resources._1416378199_notification_warning,
            };
            popupNotifier1.Click += PopupNotifier1_Click;

            popupNotifier1.Popup();
            if (Program.NotificacaoSonoro)
                AlertaSonoro();
        }
        void AlertPopUpError(string message)
        {
            popupNotifier1 = new PopupNotifier
            {
                TitleText = "Alerta de erro!",
                ContentText = message,

                ShowCloseButton = true,
                ShowOptionsButton = true,
                ShowGrip = true,
                Delay = 9000,
                AnimationInterval = 10,
                AnimationDuration = 200,
                TitlePadding = new Padding(0),
                ContentPadding = new Padding(0),
                ImagePadding = new Padding(0),
                Scroll = true,
                Image = Resources._1416378199_notification_warning,
            };
            popupNotifier1.Click += PopupNotifier1_Click;

            popupNotifier1.Popup();

            AlertaSonoroError();
        }
        private void PopupNotifier1_Click(object sender, EventArgs e)
        {

            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Normal;
                popupNotifier1.Hide();
                //btnPedidoNovos.PerformClick();
            }
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (
                MessageBox.Show("Ao sair do sistema implicará no monitoramento de novos pedidos." + Environment.NewLine +
                "Você tem certeza que deseja sair?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void AlertaSonoro()
        {

            var operation = AsyncOperationManager.CreateOperation(null);
            var thread = new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    var sound = Resources.bell;
                    // var localSound = $"{Environment.CurrentDirectory}\\bell.wav";
                    var simpleSound = new SoundPlayer(sound);

                    simpleSound.Play();

                    Thread.Sleep(2000);

                    simpleSound.Play();
                }
                catch (Exception ex)
                {
                    operation.PostOperationCompleted(ReadError, ex);
                }
            }));
            thread.Start();
        }
        private void AlertaSonoroError()
        {
            var operation = AsyncOperationManager.CreateOperation(null);
            var thread = new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    var sound = Resources.Smoke_Error;
                    // var localSound = $"{Environment.CurrentDirectory}\\bell.wav";
                    var simpleSound = new SoundPlayer(sound);

                    simpleSound.Play();

                    Thread.Sleep(2000);

                    simpleSound.Play();
                }
                catch (Exception ex)
                {
                    operation.PostOperationCompleted(ReadError, ex);
                }
            }));
            thread.Start();
        }

        private void produtoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormCadastroProdutos())
            {
                form.ShowDialog();
            }
        }

        private void abrirLojaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AVAILABLE OR UNAVAILABLE
            var result = _pedidoService.StatusLoja("AVAILABLE", "");

            MessageBox.Show(result
                ? "Loja aberta com sucesso."
                : "Ocorreu problema ao abrir a loja.\nEntre em contato com o suporte.");
        }

        private void fecharLojaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var motivo = FormMotivo.ShowFormBox("Motivo fechamento.");
            //AVAILABLE OR UNAVAILABLE
            var result = _pedidoService.StatusLoja("UNAVAILABLE", motivo);
            MessageBox.Show(result
                ? "Loja fechada com sucesso."
                : "Ocorreu problema ao fechar a loja.\nEntre em contato com o suporte.");
        }

        private void atualizrProdutosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = _pedidoService.AtualizaPrecoProduto(decimal.Parse("6.5"), "1");
            MessageBox.Show(result
                ? "Prouto atualizado com sucesso."
                : "Ocorreu problema ao atualizar o produto.\nEntre em contato com o suporte.");
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlNav.Height = button3.Height;
            pnlNav.Top = button3.Top;

            timerPrincipal.Stop();
            timerControles.Stop();
            timerZFood.Stop();
            timerEntregas.Stop();

            using (var form = new frmParent())
            {
                form.TopMost = false;
                form.ShowDialog();

                timerPrincipal.Start();
                timerControles.Start();
                timerEntregas.Start();

                timerZFood.Start();

                Application.Restart();
            }
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            var eventodePedidos = new List<PedidoVapVupt>();

            eventodePedidos = !string.IsNullOrEmpty(txtNumeroIfood.Text)
                ? _pedidoRepository.ObterPorAppId(txtNumeroIfood.Text).ToList()
                : _pedidoRepository.ObterPorData(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date).OrderBy(t => t.DataHora).ToList();

            flowLayoutPanel4.Controls.Clear();
            foreach (var eventoPedido in eventodePedidos)
            {

                var pedido = eventoPedido.PedidoRootModel ??
                             _pedidoIfoodService.ObterById(eventoPedido.PedidoId);

                if (pedido == null) continue;

                pedido.sitEvent = eventoPedido.Situacao.ToString();
                pedido.aplicacao = eventoPedido.Aplicacao;

                var row = new RowPedido();

                row.Cliente = $"{pedido.customer.name}";
                row.PedidoId = pedido.displayId;
                row.DataHora = pedido.DataPedido.ToString("dd-MM-yyyy HH:mm");
                // row.Endereco =
                //    $"{pedido.delivery.deliveryAddress?.formattedAddress} {pedido.delivery.deliveryAddress?.streetNumber} {pedido.delivery.deliveryAddress?.neighborhood}";
                row.Telefone = pedido.customer.phone.number;
                row.Tipo = pedido.orderType;
                row.Status = pedido.sitEvent;
                row.Aplicacao = pedido.aplicacao;

                row.ItemSource = pedido;
                row.ButtonText = "Detalhes do Pedido";
                row.ConfirmEvent += Row_Click;
                flowLayoutPanel4.Controls.Add(row);
            }

        }
        private void ReadErrorGeral(object error)
        {
            if (error is Exception)
            {
                timerPrincipal.Stop();
                timerPrincipal.Interval = 25000;
                lbStatusLoja.Text = ((Exception)error).Message;
                lbStatusLoja.ForeColor = Color.Red;
                panel1.BackColor = Color.Red;

                timerPrincipal.Start();

                AlertPopUpError(lbStatusLoja.Text);
            }
        }

        private void atualizarTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = Program.AtualizaToken();

            Funcoes.Mensagem(result ? "Token atualizado com sucesso" : "Erro ao atualizar o token", "Autenticação",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtNumeroIfood_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            btnPesquisa.PerformClick();
        }

        void button1_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnPedidoNovos.Height;
            pnlNav.Top = btnPedidoNovos.Top;
            pnlNav.Left = btnPedidoNovos.Left;            

            var form = Application.OpenForms["FormPedidoItens"];
            if (form != null) return;
            if (tablessControl1.SelectedTab == tabPedidoNovos) return;

            tablessControl1.SelectedTab = null;
            tablessControl1.SelectedTab = tabPedidoNovos;

        }

        void btnAguardandoEntrega_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnAguardandoEntrega.Height;
            pnlNav.Top = btnAguardandoEntrega.Top;

            tablessControl1.SelectedTab = null;
            tablessControl1.SelectedTab = tabAguardandoPedidos;

        }


        void btnFinalizados_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnFinalizados.Height;
            pnlNav.Top = btnFinalizados.Top;

            tablessControl1.SelectedTab = tabPesquisa;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                btnPedidoNovos.BackColor = Color.Transparent;
                btnAguardandoEntrega.BackColor = Color.Transparent;
                btnCancelados.BackColor = Color.Transparent;
                btnFinalizados.BackColor = Color.Transparent;

                if (tablessControl1.SelectedTab == tabPedidoNovos)
                {
                    CarregaPedidoNovos();
                    btnPedidoNovos.BackColor = Color.Gray;
                }

                else if (tablessControl1.SelectedTab == tabAguardandoPedidos)
                {
                    btnAguardandoEntrega.BackColor = Color.Gray;
                    CarregaPedidosPendenteEntrega();
                }

                else if (tablessControl1.SelectedTab == tabAguardandoRetorno)
                {
                    CarregaPedidosPendenteRetorno();
                }

                else if (tablessControl1.SelectedTab == tabCancelados)
                {
                    CarregaPedidosCancelados();
                    btnCancelados.BackColor = Color.Gray;
                }
                else if (tablessControl1.SelectedTab == tabPesquisa)
                {
                    btnFinalizados.BackColor = Color.Gray;
                }
            }
            catch (Exception exception)
            {

                _logWriter.LogWrite(exception.Message);
            }            

        }

        void btnCancelados_Click(object sender, EventArgs e)
        {
            pnlNav.Height = btnCancelados.Height;
            pnlNav.Top = btnCancelados.Top;

            tablessControl1.SelectedTab = tabCancelados;
        }

        private void lojaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormProfile())
            {
                form.ShowDialog();
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var restauranteService = new RestauranteService();
            restauranteService.AlterarStatusLoja("Aberto");
            Program.Restaurante = restauranteService.ObterPorToken(Program.TokenVapVupt);

            CarregaSituacaoLoja();
        }

        private void fecharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var restauranteService = new RestauranteService();
            restauranteService.AlterarStatusLoja("FechadoRestaurante");
            Program.Restaurante = restauranteService.ObterPorToken(Program.TokenVapVupt);

            CarregaSituacaoLoja();
        }

        private void categoriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormCategoria())
            {
                form.ShowDialog();
            }
        }

        private void enviarNotificaçoesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormPushNotification())
            {
                form.ShowDialog();
            }
        }

        private void cupomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormCadastroCupom())
            {
                form.ShowDialog();
            }
        }

        private void administrativoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormAdmin())
            {
                form.ShowDialog();
            }
        }

        private void grupoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FromGrupoProduto())
            {
                form.ShowDialog();
            }
        }

        private void campanhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FrmFlayerCadastro())
            {
                form.ShowDialog();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (var form = new FormRating())
            {
                form.ShowDialog();
            }
        }

        private void tablessControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            {
                Application.Exit();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            SobreZfood aboutWindow = new SobreZfood();
            aboutWindow.Show();

        }
    }
}
