using System;
using System.Windows.Forms;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm.Wizard
{
    public partial class frm4 : FormBase
    {
        public frm4()
        {
            InitializeComponent();

        }
        public override bool Concluir()
        {
            try
            {

                if (!string.IsNullOrEmpty(txtToken.Text))
                    GetValueApp.AddOrUpdateAppSettings("TokenVapVupt", txtToken.Text);
                if (!string.IsNullOrEmpty(txtCnpj.Text))
                    GetValueApp.AddOrUpdateAppSettings("CnpjLoja", txtCnpj.Text);


                return true;
            }
            catch
            {
                return false;
            }
        }

        private void frm3_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var token = GetValueApp.GetValue<string>("TokenVapVupt");
            var cnpj = GetValueApp.GetValue<string>("CnpjLoja");

            txtCnpj.Text = cnpj;
            txtToken.Text = token;

            if(string.IsNullOrEmpty(token))return;

            var restauranteService = new RestauranteService();
            var restaurante = restauranteService.ObterPorToken(txtToken.Text);

            CarregaRestaurante(restaurante);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var form = new FormProdutoCadastro())
            {
                form.TopMost = this.TopMost;
                form.ShowDialog();
            }
        }

        private void txtToken_Leave(object sender, EventArgs e)
        {
            
        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            var restauranteService = new RestauranteService();
            var restaurante = restauranteService.ObterPorCnpj(txtCnpj.Text);

            CarregaRestaurante(restaurante);
        }

        private void CarregaRestaurante(Restaurante restaurante)
        {
            if (restaurante == null)
            {
                MessageBox.Show("Restaurante não encontrado.\nVerifique o token de acesso e tente novamente");
                button1.Enabled = false;
                return;
            }
            txtCnpj.Text = restaurante.Cnpj;
            txtEmpresa.Text = restaurante.Nome;
            txtEndereco.Text = $"{restaurante.Logradouro}, {restaurante.Numero}";
            txtCidade.Text = restaurante.Cidade;
            txtToken.Text = restaurante.Token.ToString();

            button1.Enabled = true;
        }
    }
}
