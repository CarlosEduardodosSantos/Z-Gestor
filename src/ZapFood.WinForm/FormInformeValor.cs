using System;
using System.Windows.Forms;

namespace ZapFood.WinForm
{
    public partial class FormInformeValor : Form
    {
        public decimal ValorInformado => txtValor.ValueNumeric;

        public FormInformeValor()
        {
            InitializeComponent();
        }

        private void FormInformeValor_Load(object sender, EventArgs e)
        {
            txtValor.Focus();
        }

        private void txtValor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnConfirmar.PerformClick();
        }

        public static decimal ObterValor()
        {
            using (var form = new FormInformeValor())
            {
                form.ShowDialog();
                return form.ValorInformado;
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
