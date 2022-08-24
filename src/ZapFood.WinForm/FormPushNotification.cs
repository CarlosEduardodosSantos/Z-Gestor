using System;
using System.Windows.Forms;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormPushNotification : Form
    {
        private readonly PushNotificationService _notificationService;
        public FormPushNotification( )
        {
            _notificationService = new PushNotificationService();
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            var result = _notificationService.Enviar(txtCabecalho.Text, txtMensagem.Text);
            MessageBox.Show(result.Message, "Envio", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
