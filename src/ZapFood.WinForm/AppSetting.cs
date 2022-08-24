using System;
using System.Configuration;
using System.Windows.Forms;

namespace ZapFood.WinForm
{
    public class AppSetting
    {
        private Configuration config;
        private LogWriter _logWriter;
        public AppSetting()
        {
            _logWriter = new LogWriter();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string GetConnectionString(string key)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            catch (Exception ex)
            {
                Funcoes.Mensagem("Erro ao acessar o arquivo de configuração.\nPara mais informações acessse o log.", "Erro", MessageBoxButtons.OK);
                _logWriter.LogWrite($"Função GetConnectionString MSG: {ex.Message}");
            }
            return "";
            //return config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
        }

        public void SaveConnectionString(string key, string value)
        {
            config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
            config.ConnectionStrings.ConnectionStrings[key].ProviderName = "System.Data.SqlClient";
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}