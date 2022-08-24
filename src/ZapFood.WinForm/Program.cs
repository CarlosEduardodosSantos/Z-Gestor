using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Data.Service;
using ZapFood.WinForm.Data.Service.Interface;
using ZapFood.WinForm.IFoodAutenticate;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;
using ZapFood.WinForm.Wizard;

namespace ZapFood.WinForm
{
    static class Program
    {
        public static string AddressApi
        {
            get { return "https://api.zclub.com.br"; }
            //get { return "http://localhost:56435"; }
            set { }
        }
        public static string AddressApiIFood
        {
            get { return "https://merchant-api.ifood.com.br"; }
            //get { return "http://localhost:56435"; }
            set { }
        }
        public static Restaurante Restaurante;
        public static TipoDatabaseEnum TipoDatabase => (TipoDatabaseEnum)GetValueApp.GetValue<int>("TipoDatabase");
        public static string TokenVapVupt => GetValueApp.GetValue<string>("TokenVapVupt");
        public static string TokenDeliveryApp => GetValueApp.GetValue<string>("TokenDeliveryApp");
        public static string TokenIFood;
        public static DateTime DataRefresh;
        public static int Pdv => GetValueApp.GetValue<int>("Pdv");
        public static int Loja => GetValueApp.GetValue<int>("Loja");
        public static int CaixaId => VerificaCaixa();
        public static int VendedorId => GetValueApp.GetValue<int>("Vendedor");
        public static string clientId => GetValueApp.GetValue<string>("clientId");
        public static string clientSecret => GetValueApp.GetValue<string>("clientSecret");
        public static string MerchantId => GetValueApp.GetValue<string>("MerchantId");
        public static string AuthorizationCode => GetValueApp.GetValue<string>("AuthorizationCode");
        public static string AuthorizationCodeVerifier => GetValueApp.GetValue<string>("AuthorizationCodeVerifier");
        public static bool NotificacaoSonoro => Convert.ToBoolean(GetValueApp.GetValue<int>("NotificacaoSonoro"));
        public static bool AutoConfirmar => Convert.ToBoolean(GetValueApp.GetValue<int>("AutoConfirmar"));
        public static bool ImprimeGestor => Convert.ToBoolean(GetValueApp.GetValue<int>("ImprimeGestor"));
        public static TipoDescricaoEnumView TipoDescricao => (TipoDescricaoEnumView)GetValueApp.GetValue<int>("TipoDescricao");
        public static SistemaEnum Sistema => ObterSistema();
        public static string Version = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        public static string[] Monitorar => VerificaMonitoramento();
        public static MerchantModel Merchant { get; set; }
        public static TipoAplicativoEnumModel TipoAplicativoIfood => (TipoAplicativoEnumModel)GetValueApp.GetValue<int>("TipoAplicativoIfood");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        [STAThread]
        static void Main(string[] args)
        {
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-br");

            bool createdNew = true;


            if (args.Length > 0)
            {
                if (args[0] == "Install")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmParent());
                }

            }
            else
            {
                using (var mutex = new Mutex(true, "VapVupt.exe", out createdNew))
                {
                    try
                    {
                        if (createdNew)
                        {


                            try
                            {
                                Application.EnableVisualStyles();
                                Application.SetCompatibleTextRenderingDefault(false);
                                Application.DoEvents();

                                //var erro = new InstalacaoRepository().VerificaInstalacao();
                                var erro = FormSplash.InicializaAplicacao();
                                if (!erro)
                                {
                                    throw new Exception("Erro na instalação do sistema.");
                                }
                                Application.Run(new FormPrincipal());
                            }
                            catch (Exception e)
                            {
                                if (MessageBox.Show($"Encontramos um problema com a conexão ao servidor!\n{e.Message}\nDeseja configurar o sistema?",
                                        "Instalação", MessageBoxButtons.YesNo, MessageBoxIcon.Error,
                                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                {
                                    Application.Run(new frmParent(false));
                                    Application.Restart();
                                }
                                else
                                {
                                    Application.Exit();
                                }

                            }
                        }
                        else
                        {
                            var current = Process.GetCurrentProcess();
                            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                            {
                                if (process.Id != current.Id)
                                {
                                    //SetForegroundWindow(process.MainWindowHandle);
                                    break;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Application.Exit();
                    }
                }
            }
        }
        public static string GetToken(bool force = false)
        {
            var token = new AccessTokenModelView();
            var result = new GenericResult<AccessTokenModelView>();
            IAccessTokenService accessRepository = new AccessTokenService(TipoDatabase);

            var configuracao = accessRepository.ObterVariavel("Ifood");
            if (configuracao != null)
            {
                Program.DataRefresh = configuracao.dataRefresh;
                TokenIFood = configuracao.accesstoken;

                if (Program.DataRefresh >= DateTime.Now && Program.DataRefresh != DateTime.MinValue && !force)
                    return TokenIFood;

                result = ClientHelper.GetToken(clientId, clientSecret);

                if (!result.Success)
                {
                    var tokenRefresh = configuracao.refreshToken ?? configuracao?.accesstoken;
                    result = ClientHelper.RefreshToken(clientId, clientSecret, tokenRefresh);
                }

                if (!result.Success) return string.Empty;


            }
            else
            {
                result = ClientHelper.GetToken(clientId, clientSecret);
                if (!result.Success) return string.Empty;

            }

            token = result.Result;
            token.tokensystem = "Ifood";
            token.datacreate = DateTime.Now;
            accessRepository.Adicionar(token);

            Program.DataRefresh = token.dataRefresh;

            return token.accesstoken;


        }
        public static bool AtualizaToken()
        {
            GetToken();

            return true;
        }
        public static int VerificaCaixa()
        {
            try
            {
                if (TipoDatabase == TipoDatabaseEnum.SQLite) return 999;

                var caixaId = new CaixaRepository().ObterCaixaId();
                return caixaId;
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    $"Encontramos um problema!\n{e.Message}\nVerifique sua rede ou entre em contato com a Zip Software",
                    "Falha geral");
                return 0;
            }

        }

        private static SistemaEnum ObterSistema()
        {
            return new SistemaRepository().ObterSistema();
        }

        private static string[] VerificaMonitoramento()
        {
            var array = new string[4];

            if (ObterValorConfig("MonitoraDelivey"))
                array[0] = "DELIVERY";
            if (ObterValorConfig("MonitoraRetira"))
            {
                array[1] = "RETIRA";
                array[1] = "TAKEOUT";
                
            }
                
            if (ObterValorConfig("MonitoraMesa"))
                array[2] = "TABLE";
            if (ObterValorConfig("MonitoraToten"))
                array[3] = "TOTEN";

            //var arrayNew = (string[])array.ToArray();

            return array;
        }

        private static bool ObterValorConfig(string key)
        {
            try
            {
                String value = ConfigurationManager.AppSettings[key];
                var valueInt = (int)Convert.ChangeType(value, typeof(int));

                return Convert.ToBoolean(valueInt);
            }
            catch (Exception)
            {
                if (key == "MonitoraMesa" || key == "MonitoraToten")
                    return false;
                //Caso de excessão ´´e pq não esta configurado e retorna true para não ter problema noque ja esta funcionando
                return true;
            }
        }

    }
}
