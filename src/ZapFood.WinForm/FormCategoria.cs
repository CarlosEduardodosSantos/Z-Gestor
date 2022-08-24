using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormCategoria : Form
    {
        private readonly CategoriaService _categoriaService;
        private List<Categoria> _categorias;
        private Categoria _categoria;
        private bool _isAlter;
        public FormCategoria()
        {
            InitializeComponent();
            _categoriaService = new CategoriaService();
        }

        private void FormCategoria_Load(object sender, EventArgs e)
        {
            CarregaCategorias();

        }

        private void CarregaCategorias()
        {
            _categorias = _categoriaService.ObterCategorias(99, 1).ResultToList<Categoria>();
            dataGridView2.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = _categorias.OrderBy(t => t.Sequencia).ToList();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var index = dataGridView2.SelectedRows[0].Index;
            var categoria = (Categoria)dataGridView2.Rows[index].DataBoundItem;
            if (index == 0)
            {
                categoria.Sequencia = 1;
                _categoriaService.Alterar(categoria);
                CarregaCategorias();
                return;
            }
            

            var indexAlter = index-1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter + 2;

            var categoria2 = (Categoria)dataGridView2.Rows[indexAlter].DataBoundItem;

            categoria.Sequencia = sequenciaNovo;
            _categoriaService.Alterar(categoria);


            categoria2.Sequencia = sequenciaOld;
            _categoriaService.Alterar(categoria2);

            CarregaCategorias();

            var indexPos = _categorias.FindIndex(t => t.CategoriaId == categoria.CategoriaId);
            dataGridView2.Rows[indexPos].Selected = true;
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!_isAlter)
                _categoria = new Categoria();

            _categoria.RestauranteId = Program.Restaurante.RestauranteId;
            _categoria.RestauranteToken = Program.TokenVapVupt;
            _categoria.Descricao = textBox1.Text;
            _categoria.Situacao = (SituacaoCadastro) comboBox1.SelectedIndex + 1;
            _categoria.Sequencia = _isAlter ? _categoria.Sequencia : dataGridView2.RowCount + 1;


            var result = !_isAlter
                ? _categoriaService.Adicionar(_categoria)
                : _categoriaService.Alterar(_categoria);

            _isAlter = false;
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;

            MessageBox.Show(result, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CarregaCategorias();

            var indexPos = _categorias.FindIndex(t => t.Descricao == _categoria.Descricao);
            dataGridView2.Rows[indexPos].Selected = true;


        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var index = dataGridView2.SelectedRows[0].Index;
            _categoria = (Categoria)dataGridView2.Rows[index].DataBoundItem;
            textBox1.Text = _categoria.Descricao;
            comboBox1.SelectedIndex = (int)_categoria.Situacao - 1;
            _isAlter = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var index = dataGridView2.SelectedRows[0].Index;
            var categoria = (Categoria)dataGridView2.Rows[index].DataBoundItem;
            if (index == dataGridView2.RowCount-1)
            {
                return;
            }


            var indexAlter = index + 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter;

            var categoria2 = (Categoria)dataGridView2.Rows[indexAlter].DataBoundItem;

            categoria.Sequencia = sequenciaNovo;
            _categoriaService.Alterar(categoria);


            categoria2.Sequencia = sequenciaOld;
            _categoriaService.Alterar(categoria2);

            CarregaCategorias();

            var indexPos = _categorias.FindIndex(t => t.CategoriaId == categoria.CategoriaId);
            dataGridView2.Rows[indexPos].Selected = true;
        }
    }
}
