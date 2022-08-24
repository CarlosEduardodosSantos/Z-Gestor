using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;

namespace ZapFood.WinForm
{
    public partial class RelacionarProduto : Form
    {
        private List<Produto> _produtos;
        private Produto _produto;
        public RelacionarProduto(string produto)
        {
            InitializeComponent();
            lbProduto.Text = produto;
            textBox1.Select();
        }
        public static Produto Relacionar(string produto)
        {
            using (var form = new RelacionarProduto(produto))
            {
                form.ShowDialog();
                return form._produto;
            }

            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var codigo = 0;
            var descricao = string.Empty;
            if (Funcoes.Val_NumeroKey(textBox1.Text))
                codigo = Convert.ToInt32(textBox1.Text);
            else
                descricao = textBox1.Text;

            _produtos = new ProdutoRepository().ObterPorNome(descricao, codigo).ToList();

            if(_produtos.Count == 0)return;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _produtos;
            dataGridView1.Select();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)return;

            _produto = _produtos[dataGridView1.SelectedRows[0].Index];
            Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode != Keys.Enter)return;

            button1.PerformClick();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            

            if (e.KeyCode != Keys.Enter) return;

            e.SuppressKeyPress = true;

            _produto = _produtos[dataGridView1.SelectedRows[0].Index];
            Close();
        }
    }
}
