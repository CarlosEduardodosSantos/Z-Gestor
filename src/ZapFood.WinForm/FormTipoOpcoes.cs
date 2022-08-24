using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormTipoOpcoes : Form
    {
        private readonly ProdutoOpcaoService _produtoOpcaoService;
        private List<ProdutoOpcaoTipo> _tipos;
        private ProdutoOpcaoTipo _tipo;
        private bool _isAlter;
        public FormTipoOpcoes()
        {
            InitializeComponent();
            _produtoOpcaoService = new ProdutoOpcaoService();
        }

        private void FormTipoOpcoes_Load(object sender, EventArgs e)
        {
            CarregaOpcoes();
        }

        void CarregaOpcoes()
        {
            _tipos = _produtoOpcaoService.ObterByTipos().ResultToList<ProdutoOpcaoTipo>().OrderBy(o => o.Sequencia).ToList();

            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.DataSource = _tipos;
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtDescricao.Text))
            {
                Funcoes.Mensagem("É obrigatório informar o descrição do tipo", "Validação", MessageBoxButtons.OK);
                return;
            }
            if (cbTipo.SelectedIndex == -1)
            {
                Funcoes.Mensagem("É obrigatório informar o tipo.", "Validação", MessageBoxButtons.OK);
                return;
            }
            try
            {
                if (!_isAlter)
                {
                    var tipo = new ProdutoOpcaoTipo()
                    {
                        Nome = txtDescricao.Text,
                        Quantidade = int.Parse(txtQtdeMin.ValueNumeric.ToString("N0")),
                        QtdeMax = int.Parse(txtQtdeMax.ValueNumeric.ToString("N0")),
                        Obrigatorio = chkObrigatorio.Checked,
                        Sequencia = dataGridView3.RowCount + 1,
                        Tipo = cbTipo.SelectedIndex + 1
                    };

                    _produtoOpcaoService.AdicionarTipo(tipo);
                }
                else
                {
                    _tipo.Nome = txtDescricao.Text;
                    _tipo.Quantidade = int.Parse(txtQtdeMin.ValueNumeric.ToString("N0"));
                    _tipo.QtdeMax = int.Parse(txtQtdeMax.ValueNumeric.ToString("N0"));
                    _tipo.Obrigatorio = chkObrigatorio.Checked;
                    _tipo.Tipo = cbTipo.SelectedIndex + 1;
                    var result = _produtoOpcaoService.AlterarTipo(_tipo);
                    if (!string.IsNullOrEmpty(result))
                        MessageBox.Show(result, "Alterar cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        throw new Exception("Ocorreu um erro na alteração.\nVerifique os dados e tente novamente.");
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Alterar cadastro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            CarregaOpcoes();

            txtDescricao.Clear();
            txtQtdeMin.ValueNumeric = 0;
            txtQtdeMax.ValueNumeric = 0;
            chkObrigatorio.Checked = false;
            _isAlter = false;

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            var index = dataGridView3.SelectedRows[0].Index;
            var tipo = (ProdutoOpcaoTipo)dataGridView3.Rows[index].DataBoundItem;
            if (index == 0)
            {
                tipo.Sequencia = 1;
                _produtoOpcaoService.AlterarTipo(tipo);
                CarregaOpcoes();
                return;
            }


            var indexAlter = index - 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter + 2;

            var tipo2 = (ProdutoOpcaoTipo)dataGridView3.Rows[indexAlter].DataBoundItem;

            tipo.Sequencia = sequenciaNovo;
            _produtoOpcaoService.AlterarTipo(tipo);


            tipo2.Sequencia = sequenciaOld;
            _produtoOpcaoService.AlterarTipo(tipo2);

            CarregaOpcoes();

            var indexPos = _tipos.FindIndex(t => t.ProdutosOpcaoTipoId == tipo.ProdutosOpcaoTipoId);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var index = dataGridView3.SelectedRows[0].Index;
            var tipo = (ProdutoOpcaoTipo)dataGridView3.Rows[index].DataBoundItem;
            if (index == dataGridView3.RowCount - 1)
            {
                return;
            }


            var indexAlter = index + 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter;

            var tipo2 = (ProdutoOpcaoTipo)dataGridView3.Rows[indexAlter].DataBoundItem;

            tipo.Sequencia = sequenciaNovo;
            _produtoOpcaoService.AlterarTipo(tipo);


            tipo2.Sequencia = sequenciaOld;
            _produtoOpcaoService.AlterarTipo(tipo2);

            CarregaOpcoes();

            var indexPos = _tipos.FindIndex(t => t.ProdutosOpcaoTipoId == tipo.ProdutosOpcaoTipoId);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var index = e.RowIndex;
            if (index == -1)return;

            var data = (ProdutoOpcaoTipo)dataGridView3.Rows[index].DataBoundItem;
            if (e.ColumnIndex == 4)
            {
                data.Situacao = data.Situacao == 0 ? 1: 0;
                _produtoOpcaoService.AlterarTipo(data);

                //Altera

            }
            else if (e.ColumnIndex == 5)
            {
                //Deleta
                if (MessageBox.Show($"Confirma exclusão do item {data.Nome}?", "Exclusão", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No) return;

                _produtoOpcaoService.ExcluirTipo(data);
            }
            else
            {
                CarregaData(data);
                return;
            }

            CarregaOpcoes();
        }

        private void CarregaData(ProdutoOpcaoTipo data)
        {
            _tipo = data;
            txtDescricao.Text = data.Nome;
            txtQtdeMin.ValueNumeric = data.Quantidade;
            txtQtdeMax.ValueNumeric = data.QtdeMax;
            chkObrigatorio.Checked = data.Obrigatorio;
            cbTipo.SelectedIndex  = data.Tipo == 3 ? 1 : data.Tipo- 1;
            _isAlter = true;
        }

        private void dataGridView3_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            var index = e.Row.Index;
            if (index == -1) return;
            try
            {
                var data = (ProdutoOpcaoTipo)dataGridView3.Rows[index].DataBoundItem;

                dataGridView3.Rows[index].Cells[4].Value = data.Situacao == 1 ? Properties.Resources.pause_16 : Properties.Resources.play_16;
                dataGridView3.Rows[index].DefaultCellStyle.BackColor = data.Situacao == 1 ? Color.LightGreen : Color.LightCoral;
            }
            catch (Exception exception)
            {
                
            }

        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            txtDescricao.Clear();
            txtQtdeMin.ValueNumeric = 0;
            txtQtdeMax.ValueNumeric = 0;
            chkObrigatorio.Checked = false;
            cbTipo.SelectedIndex = -1;
            _isAlter = false;
        }
    }
}
