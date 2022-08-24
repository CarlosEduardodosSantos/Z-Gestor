using System;
using System.Linq;

using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm.Wizard
{
    public partial class frm3 : FormBase
    {
        private UserCodeModel userCodeModel;
        public frm3()
        {
            InitializeComponent();

        }
        public override bool Concluir()
        {
            try
            {
                if (!string.IsNullOrEmpty(cbLoja.Text))
                    GetValueApp.AddOrUpdateAppSettings("Loja", cbLoja.Text);
                if (!string.IsNullOrEmpty(cbPdv.Text))
                    GetValueApp.AddOrUpdateAppSettings("Pdv", cbPdv.Text);
                if (!string.IsNullOrEmpty(cbVendedor.Text))
                    GetValueApp.AddOrUpdateAppSettings("Vendedor", ((int?)cbVendedor.SelectedValue ?? 0).ToString());
                 if (!string.IsNullOrEmpty(txtAuthorizationCode.Text))
                     GetValueApp.AddOrUpdateAppSettings("AuthorizationCode", txtAuthorizationCode.Text);
                 if (!string.IsNullOrEmpty(txtAuthorizationCodeVerifier.Text))
                     GetValueApp.AddOrUpdateAppSettings("AuthorizationCodeVerifier", txtAuthorizationCodeVerifier.Text);
                if (!string.IsNullOrEmpty(txtMerchantId.Text))
                    GetValueApp.AddOrUpdateAppSettings("MerchantId", txtMerchantId.Text);
                if (!string.IsNullOrEmpty(cbTipoIntegracao.Text))
                    GetValueApp.AddOrUpdateAppSettings("TipoAplicativoIfood", cbTipoIntegracao.SelectedIndex.ToString());



                return true;
            }
            catch
            {
                return false;
            }
        }

        private void frm3_Load(object sender, EventArgs e)
        {


            var loja = GetValueApp.GetValue<int>("Loja");
            var pdv = GetValueApp.GetValue<int>("Pdv");
            var vendedorId = GetValueApp.GetValue<int>("Vendedor");
            var username = GetValueApp.GetValue<string>("clientId");
            var password = GetValueApp.GetValue<string>("clientSecret");
            var merchantId = GetValueApp.GetValue<string>("MerchantId");
            var authorizationCode = GetValueApp.GetValue<string>("AuthorizationCode");
            var authorizationCodeVerifier = GetValueApp.GetValue<string>("AuthorizationCodeVerifier");

            var tipoDatabase = (TipoDatabaseEnum)GetValueApp.GetValue<int>("TipoDatabase");
            var tipoIntegracao = (int)GetValueApp.GetValue<int>("TipoAplicativoIfood");


            txtAuthorizationCode.Text = authorizationCode;
            txtAuthorizationCodeVerifier.Text = authorizationCodeVerifier;
            txtMerchantId.Text = merchantId;
            cbTipoIntegracao.SelectedIndex = tipoIntegracao;

            if (tipoDatabase == TipoDatabaseEnum.SQLite)
            {
                cbVendedor.Enabled = false;
                cbLoja.Enabled = false;
                cbPdv.Enabled = false;
                return;
            }


            var vendedorRepository = new VendedorRepository();
            var vendedores = vendedorRepository.ObterTodos().ToList();

            cbVendedor.DataSource = vendedores;
            cbVendedor.ValueMember = "VendedorId";
            cbVendedor.DisplayMember = "Nome";
            cbVendedor.SelectedValue = vendedorId;

            cbLoja.SelectedIndex = loja - 1;
            cbPdv.SelectedIndex = pdv - 1;


        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            using (var appService = new MerchantService())
            {
                userCodeModel = appService.ObterLinkIntegracao();

                if (string.IsNullOrEmpty(userCodeModel.authorizationCodeVerifier))
                {
                    Funcoes.Mensagem("Erro ao gerar o código para integração!", "iFood", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return;
                }

                txtLinkIntegracao.Text = userCodeModel.verificationUrlComplete;
                txtLinkIntegracao.Visible = true;


                txtAuthorizationCodeVerifier.Text = userCodeModel.authorizationCodeVerifier;


                System.Diagnostics.Process.Start(userCodeModel.verificationUrlComplete);
            }
        }

        private void cbTipoIntegracao_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtAuthorizationCode.Enabled = cbTipoIntegracao.SelectedIndex == 0;
            txtAuthorizationCodeVerifier.Enabled = cbTipoIntegracao.SelectedIndex == 0;
            if (cbTipoIntegracao.SelectedIndex == 1)
            {
                txtAuthorizationCode.Clear();
                txtAuthorizationCodeVerifier.Clear();
            }
        }

    }
}
