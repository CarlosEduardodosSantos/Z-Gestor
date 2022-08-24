using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ZapFood.WinForm.Wizard
{
    public partial class form5 : FormBase
    {
        public form5()
        {
            InitializeComponent();
        }

        private void form5_Load(object sender, System.EventArgs e)
        {
            var imprimeGestor = GetValueApp.GetValue<int>("ImprimeGestor");
            var descricaoApp = GetValueApp.GetValue<int>("TipoDescricao");
            var notificacao = GetValueApp.GetValue<int>("NotificacaoSonoro");
            var autoConfirmar = GetValueApp.GetValue<int>("AutoConfirmar");

            var monitoraDelivey = GetValueApp.GetValue<int>("MonitoraDelivey");
            var monitoraRetira = GetValueApp.GetValue<int>("MonitoraRetira");
            var monitoraMesa = GetValueApp.GetValue<int>("MonitoraMesa");
            var monitoraToten = GetValueApp.GetValue<int>("MonitoraToten");


            chkImprimeGestor.Checked = Convert.ToBoolean(imprimeGestor);
            chkUsaDesApp.Checked = Convert.ToBoolean(descricaoApp);
            chkNotificacao.Checked = Convert.ToBoolean(notificacao);
            chkAutoConfirmar.Checked = Convert.ToBoolean(autoConfirmar); 

            chkMoniDelivey.Checked = Convert.ToBoolean(monitoraDelivey);
            chkMoniRetira.Checked = Convert.ToBoolean(monitoraRetira);
            chkMoniMesa.Checked = Convert.ToBoolean(monitoraMesa);
            chkMoniToten.Checked = Convert.ToBoolean(monitoraToten);
        }

        public override bool Concluir()
        {
            GetValueApp.AddOrUpdateAppSettings("ImprimeGestor", Convert.ToInt32(chkImprimeGestor.Checked).ToString());

            GetValueApp.AddOrUpdateAppSettings("TipoDescricao", Convert.ToInt32(chkUsaDesApp.Checked).ToString());

            GetValueApp.AddOrUpdateAppSettings("NotificacaoSonoro", Convert.ToInt32(chkNotificacao.Checked).ToString());

            GetValueApp.AddOrUpdateAppSettings("AutoConfirmar", Convert.ToInt32(chkAutoConfirmar.Checked).ToString());

            return true;
        }

        private void chkIniciaWindows_CheckedChanged(object sender, EventArgs e)
        {
            SetStartup(chkIniciaWindows.Checked);
        }

        private void SetStartup(bool OnOff)
        {
            try
            {
                //Nome a ser exibido no registro ou quando Der MSCONFIG - Pode Alterar
                string appName = "ZipFood Gestor";

                //Diretorio da chave do Registro NAO ALTERAR
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

                //Abre o registro
                RegistryKey startupKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                //Valida se vai incluir o iniciar com o Windows ou remover
                if (OnOff)//Iniciar
                {
                    if (startupKey.GetValue(appName) == null)
                    {
                        // Add startup reg key
                        startupKey.SetValue(appName, @"""" + Application.ExecutablePath.ToString() + @"""");
                        startupKey.Close();
                    }

                }
                else//Nao iniciar mais
                {
                    // remove startup
                    startupKey = Registry.LocalMachine.OpenSubKey(runKey, true);
                    startupKey.DeleteValue(appName, false);
                    startupKey.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkMoniDelivey_CheckedChanged(object sender, EventArgs e)
        {
            var tag = int.Parse(((CheckBox) sender).Tag.ToString());

            switch (tag)
            {
                case 0:
                    GetValueApp.AddOrUpdateAppSettings("MonitoraDelivey", Convert.ToInt32(chkMoniDelivey.Checked).ToString());
                    break;
                case 1:
                    GetValueApp.AddOrUpdateAppSettings("MonitoraRetira", Convert.ToInt32(chkMoniRetira.Checked).ToString());
                    break;
                case 2:
                    GetValueApp.AddOrUpdateAppSettings("MonitoraMesa", Convert.ToInt32(chkMoniMesa.Checked).ToString());
                    break;
                case 3:
                    GetValueApp.AddOrUpdateAppSettings("MonitoraToten", Convert.ToInt32(chkMoniToten.Checked).ToString());
                    break;
            }
        }
    }
}
