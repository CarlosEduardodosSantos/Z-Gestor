using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZapFood.WinForm
{
    public partial class FormMotivo : Form
    {
        public string Motivo => textBox1.Text;
        public string TextForm { 
            set => Text = value;
        }
        public FormMotivo(string textFrm)
        {
            InitializeComponent();
            TextForm = textFrm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static string ShowFormBox(string textForm)
        {
            using (var form = new FormMotivo(textForm))
            {
                form.ShowDialog();
                return form.Motivo;
            }
        }
    }
}
