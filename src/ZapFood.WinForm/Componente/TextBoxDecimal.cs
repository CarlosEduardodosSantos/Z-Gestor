using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ZapFood.WinForm.Componente
{
    public partial class TextBoxDecimal : TextBox
    {
        public TextBoxDecimal()
        {
            InitializeComponent();
            TextAlign = HorizontalAlignment.Right;
        }

        private bool Val_Numero(string _text)
        {
            Regex er = new Regex("^[0-9,\\-\b8]");
            //Regex er = new Regex("^-?\\d*(\\.\\d+)?$");
            if (er.Match(_text).Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {   
            if (Val_Numero(e.KeyChar.ToString()))
                e.Handled = false;
            else
                e.Handled = true;
        }

        public decimal ValueNumeric
        {
            get { return (!String.IsNullOrEmpty(Text.Replace("R$ ", "").Replace("R$", "").Trim())) ? Convert.ToDecimal(Text.Replace("R$ ", "").Replace("R$", "").Trim()) : 0; }
            set { Text = value.ToString(FormatDecimal + CasasDecimais); }
        }

        protected override void OnEnter(System.EventArgs e)
        {
            oldBackColor = BackColor;
            BackColor = BackColorEnter;
            if (!string.IsNullOrEmpty(Text))
            {
                if (Convert.ToDecimal(Text.Replace("R$ ", "").Replace("R$", "")) == 0)
                    Text = "";
            }
        }

        protected override void OnLeave(System.EventArgs e)
        {
            if (Text.Trim() != string.Empty)
                Text = Convert.ToDecimal(Text.Replace("R$ ", "").Replace("R$", "")).ToString(CasasDecimais);
            else
                Text = "0,00";

            BackColor = oldBackColor;
        }

        [DefaultValue("2")]
        public string CasasDecimais { get; set; }

        private Color _backColorEnter;

        public Color BackColorEnter
        {
            get { return _backColorEnter; }
            set { _backColorEnter = value; }
        }

        private Color oldBackColor;


        [DefaultValue("2")]
        public string FormatDecimal { get; set; }
    }
}
