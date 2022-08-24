using System;
using System.Windows.Forms;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Wizard
{
    public partial class frm2 : FormBase
    {
        public frm2()
        {
            InitializeComponent();

            var tipoDatabase = (TipoDatabaseEnum)GetValueApp.GetValue<int>("TipoDatabase");
            checkBox1.Checked = tipoDatabase == TipoDatabaseEnum.SQLSever;

            if (tipoDatabase == TipoDatabaseEnum.SQLite)return;

            var app = new AppSetting();
            var connect = app.GetConnectionString("MyContext");
            var indexStart = connect.IndexOf("Data Source=");

            string[] quebra = connect.Split(new char[] { ';', '=' });

            if(quebra.Length < 9)return;

            cbServidor.Text = quebra[1];
            txtDataBase.Text = quebra[3];
            txtUsername.Text = quebra[5];
            txtPassword.Text = quebra[7];

        }

        public override bool Concluir()
        {
            var connectionString = $"Data Source={cbServidor.Text}; Initial Catalog={txtDataBase.Text};user id={txtUsername.Text};password={txtPassword.Text};";

            try
            {

                GetValueApp.AddOrUpdateAppSettings("TipoDatabase", checkBox1.Checked ? "1" : "2");

                if (!checkBox1.Checked) return true;


                var sqlHelper = new SqlHelper(connectionString);
                var isConected = sqlHelper.IsConected;
                if (isConected)
                {
                    var app = new AppSetting();
                    app.SaveConnectionString("MyContext", connectionString);


                    sqlHelper.CriarTabelas();
                    
                    Funcoes.Mensagem("Muito bem! A conexão esta correta\nVamos atualizar o seu sistema agora.", "Configuração", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    Funcoes.Mensagem("Não é possivel conectar ao servidor.\nVerifique as informações digitadas e tente novamente.", "Configuração", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                Funcoes.Mensagem(exception.Message, "Configuração", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            cbServidor.Enabled = checkBox1.Checked;
            txtDataBase.Enabled = checkBox1.Checked;
            txtUsername.Enabled = checkBox1.Checked;
            txtPassword.Enabled = checkBox1.Checked;
        }
    }
}
