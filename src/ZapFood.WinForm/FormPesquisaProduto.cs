using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;

namespace ZapFood.WinForm
{
    public partial class FormPesquisaProduto : Form
    {
        private List<Produto> _produtos;
        public Produto Produto;
        private readonly string _pesquisa;
        public FormPesquisaProduto(string pesquisa)
        {
            _pesquisa = pesquisa;
            InitializeComponent();
            Load += FormPesquisaProduto_Load;
        }

        private void FormPesquisaProduto_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_pesquisa)) return;

            var produtoId = Funcoes.Val_NumeroKey(_pesquisa) ? int.Parse(_pesquisa) : 0;
            var descricao = produtoId == 0 ? _pesquisa : "";

            _produtos = new ProdutoRepository().ObterPorNomeOrId(descricao, produtoId).ToList();
            if (_produtos.Count == 1)
            {
                Produto = _produtos[0];
                Close();
            }

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _produtos;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Dimiss(e.RowIndex);
        }

        private void Dimiss(int index)
        {
            if (index == -1) return;

            Produto = _produtos[index];

            Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode != Keys.Enter) return;

            var index = dataGridView1.SelectedRows[0].Index;

            Dimiss(index);
        }
    }
}
