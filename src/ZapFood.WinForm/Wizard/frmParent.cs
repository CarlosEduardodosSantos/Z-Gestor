using System;
using System.Linq;
using System.Windows.Forms;

namespace ZapFood.WinForm.Wizard
{
    public partial class frmParent : Form
    {
        FormBase[] frm = { new frm1(), new frm2(), new frm3(), new form5(), new frm4(), new frm6() };
        int top = -1;
        int count;

        public frmParent(bool topMost = true)
        {
            count = frm.Count();
            InitializeComponent();
            this.TopMost = topMost;
        }
        private void LoadNewForm()
        {
            frm[top].TopLevel = false;
            frm[top].AutoScroll = true;
            frm[top].Dock = DockStyle.Fill;
            this.pnlContent.Controls.Clear();
            this.pnlContent.Controls.Add(frm[top]);
            frm[top].Show();
        }

        private void Back()
        {
            top--;

            if (top <= -1)
            {
                return;
            }
            else
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
                LoadNewForm();
                if (top - 1 <= -1)
                {
                    btnBack.Enabled = false;
                }
            }

            if (top >= count)
            {
                btnNext.Enabled = false;
            }
        }
        private void Next()
        {
            if (top >= 0)
            {
                if(!frm[top].Concluir())
                    return;
            }
            

            top++;
            if (top >= count)
            {
                return;
            }
            else
            {
                btnBack.Enabled = true;
                btnNext.Enabled = true;
                btnCancel.Text = "Cancelar";
                LoadNewForm();
                if (top + 1 == count)
                {
                    btnNext.Enabled = false;
                    btnCancel.Text = "Finalizar";
                }
            }

            if (top <= 0)
            {
                btnBack.Enabled = false;
            }
        }

        private void frmParent_Load(object sender, EventArgs e)
        {
            Next();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Back();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (btnCancel.Text == "Finalizar")
                frm[top].Concluir();

            this.Close();
        }
    }
}
