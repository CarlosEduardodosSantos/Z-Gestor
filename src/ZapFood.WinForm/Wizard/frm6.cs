using System;
using System.Windows.Forms;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm.Wizard
{
    public partial class frm6 : FormBase
    {
        public frm6()
        {
            InitializeComponent();

        }
        public override bool Concluir()
        {
            try
            {

                if (!string.IsNullOrEmpty(txtToken.Text))
                    GetValueApp.AddOrUpdateAppSettings("TokenDeliveryApp", txtToken.Text);


                return true;
            }
            catch
            {
                return false;
            }
        }

        private void frm6_Load(object sender, EventArgs e)
        {

            var token = GetValueApp.GetValue<string>("TokenDeliveryApp");
            txtToken.Text = token;

        }



        private void txtToken_Leave(object sender, EventArgs e)
        {
            
        }

    }
}
