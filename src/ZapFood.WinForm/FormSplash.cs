using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Model;

using System.Runtime.InteropServices;

using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormSplash : Form
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

        public bool Inicializado = false;
        string _label;
        public FormSplash()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 50, 50));
        }

        public static bool InicializaAplicacao()
        {

            using (var form = new FormSplash())
            {
                form.ShowDialog();
                return form.Inicializado;
            }
            
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Verifica a conexao com o banco de dados
                _label = "Carregando sua conexão com o banco de dados";
                Thread.Sleep(1000);
                backgroundWorker1.ReportProgress(Convert.ToInt32(1 * 100 / 3));
                var erro = Program.TipoDatabase != TipoDatabaseEnum.SQLSever || new InstalacaoRepository().VerificaInstalacao();
                if (!erro)
                    throw new Exception("Erro ao conectar com o servidor.");

                if (Program.TipoDatabase == TipoDatabaseEnum.SQLSever)
                {
                    var versao = new InstalacaoRepository().GetVersao();
                    if (versao == null)
                    {
                        var buildDate = DateTime.Now;

                        versao = new VapVupVersao();
                        versao.VersaoAtual = Program.Version;
                        versao.DataAtualizacao = buildDate;

                        new InstalacaoRepository().InsertVersao(versao);
                        new InstalacaoRepository().Update();
                    }
                    else if (Program.Version != versao.VersaoAtual)
                    {
                        versao.VersaoAtual = Program.Version;
                        versao.DataAtualizacao = DateTime.Now;

                        new InstalacaoRepository().Update();
                        new InstalacaoRepository().DefineVersao(versao);
                    }
                }
                

                Thread.Sleep(1000);
                _label = "Verificando a disponibilidade com o iFood";
                backgroundWorker1.ReportProgress(Convert.ToInt32(2 * 100 / 3));


                if (!string.IsNullOrEmpty(Program.MerchantId))
                {
                    var token = Program.GetToken();
                    if (string.IsNullOrEmpty(token))
                        throw new Exception("Erro na autenticação com o iFood");
                }

                if (!string.IsNullOrEmpty(Program.TokenVapVupt))
                {
                    try
                    {
                        var restauranteService = new RestauranteService();
                        Program.Restaurante = restauranteService.ObterPorToken(Program.TokenVapVupt);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }

                }


                Thread.Sleep(1000);
                _label = "Tudo certo com suas configurações!";
                backgroundWorker1.ReportProgress(Convert.ToInt32(3 * 100 / 3));
                Thread.Sleep(2000);
                Inicializado = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw new Exception(exception.Message);
            }


        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lbStatus.Text = _label;
        }

    }
}
