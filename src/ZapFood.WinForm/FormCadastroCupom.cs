using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormCadastroCupom : Form
    {
        private readonly CupomService _cupomService;
        private List<Cupom> _cupoms;
        private Cupom _cupom;
        public FormCadastroCupom()
        {
            InitializeComponent();
            Load += FormCadastroCupom_Load;
            _cupomService = new CupomService();
        }

        private void FormCadastroCupom_Load(object sender, EventArgs e)
        {
            tablessControl1.SelectedTab = tabPage1;

            CarregaCupons();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tablessControl1.SelectedTab = tabPage1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tablessControl1.SelectedTab = tabPage2;
            LimpaObjetos();
        }

        private void LimpaObjetos()
        {
            cbSituacao.SelectedIndex = -1;
            txtNome.Clear();
            txtDescricao.Clear();
            txtValor.Clear();
            txtValorMinimo.Clear();
            txtQuantidade.Clear();
            txtCodigo.Clear();
            txtPercentual.Clear();
            dtValidade.Value = DateTime.Now;
            checkBox1.Checked = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.SelectedRows.Count <= 0)return;

            CarregaCupomDetalhe();
            tablessControl1.SelectedTab = tabPage3;

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        void CarregaCupons()
        {
            _cupoms = _cupomService.ObterTodos().ResultToList<Cupom>().OrderBy(t => t.Situacao).ThenByDescending(t=> t.DataHora).ToList();


            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _cupoms;
        }

        void CarregaCupomDetalhe()
        {
            _cupom = (Cupom)dataGridView1.SelectedRows[0].DataBoundItem;

            button2.Text = _cupom.Situacao == 1 ? "Desativar" : "Ativar";
            button2.Tag = _cupom.Situacao;
            button2.BackColor = _cupom.Situacao == 1 ? Color.Red : Color.Green;


            lbCupomNome.Text = _cupom.Nome;
            lbValidade.Text = $"Validade: {_cupom.Validade.ToString("dd/MM/yyyy HH:mm")}";
            
            var cupomMovimentacoes = _cupomService.ObterCupomMovimentacao(_cupom.CupomId.ToString()).ResultToList<CupomMovimentacao>();
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = cupomMovimentacoes;

            var qtde = cupomMovimentacoes.Count;

            lbQtde.Text = $"Disponivel: {(_cupom.Quantidade- qtde).ToString("N0")}";
            lbQtdeUso.Text = $"Uso: {qtde.ToString("N0")}";
            lbValor.Text = $"Valor: {cupomMovimentacoes.Sum(t=> t.Valor).ToString("N2")}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var cupom = new Cupom();

            cupom.Tipo = cbSituacao.SelectedIndex+1;
            cupom.Situacao = 1;
            cupom.Nome = txtNome.Text;
            cupom.Descricao = txtDescricao.Text;
            cupom.Valor = txtValor.ValueNumeric;
            cupom.ValorMinimo = txtValorMinimo.ValueNumeric;
            cupom.Quantidade = txtQuantidade.ValueNumeric > 0 ? int.Parse(txtQuantidade.ValueNumeric.ToString()) : 0;
            cupom.Validade = dtValidade.Value;
            cupom.DataHora = DateTime.Now;
            cupom.RestauranteId = Program.Restaurante.RestauranteId;
            cupom.Percentual = txtPercentual.ValueNumeric;
            cupom.NoLImited = checkBox2.Checked;
            cupom.Codigo = txtCodigo.Text;
            checkBox1.Checked = false;

            
            var result = _cupomService.Adicionar(cupom);
            MessageBox.Show(result, "Cadastro de cupom", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LimpaObjetos();
            tablessControl1.SelectedTab = tabPage1;
            CarregaCupons();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LimpaObjetos();
            tablessControl1.SelectedTab = tabPage1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            var situacao = (int) btn.Tag == 2 ? 1 : 2;

            var cupomId = _cupom.CupomId;
            var result = _cupomService.AlterarSituacao(cupomId, situacao);
            MessageBox.Show(result);
            CarregaCupons();
            CarregaCupomDetalhe();
        }
    }
}
