using System.Windows.Forms;

namespace ZapFood.WinForm
{
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
        }

        private void FormAdmin_Load(object sender, System.EventArgs e)
        {
            //Resposta a pergunta http://pt.stackoverflow.com/questions/109853/manipular-p%C3%A1gina-com-webbrowser
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("http://www.zipsoftware.ddns.com.br:7777/zip/zappcliente.dll/m");

           
        }
    }
}
