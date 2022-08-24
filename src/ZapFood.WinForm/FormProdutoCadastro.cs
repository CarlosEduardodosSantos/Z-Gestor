using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Subro.Controls;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormProdutoCadastro : Form
    {
        private PictureBox _image;
        private List<Produto> _produtos;
        private List<Produto> _produtosSelecionados;
        private string _textInformation;
        public FormProdutoCadastro()
        {
            InitializeComponent();
        }

        private void FormProdutoCadastro_Load(object sender, EventArgs e)
        {
            pnlProcesso.Visible = false;
            CarregaProdutos();
            _image = new PictureBox();
            _image.Load("http://zipsoftware2.ddns.com.br:56435/api/galeria/produtos/foto_em_breve.png");

        }

        private void CarregaProdutos()
        {

            _produtos = new ProdutoRepository().ObterPorNomeOrId("", 0).OrderBy(o => o.categoriaNome).ThenBy(o => o.nome).ToList();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _produtos;
           
            var grouper = new Subro.Controls.DataGridViewGrouper(dataGridView1);
            
            grouper.GroupSortOrder = SortOrder.None;
            grouper.Options.StartCollapsed = true;
            grouper.SetGroupOn("categoriaNome");
            grouper.DisplayGroup += grouper_DisplayGroup;

        }
        void grouper_DisplayGroup(object sender, GroupDisplayEventArgs e)
        {
            e.BackColor = (e.Group.GroupIndex % 2) == 0 ? Color.LightGray : Color.LightSkyBlue;
            e.Header = "";
            e.DisplayValue = e.DisplayValue;
            e.Summary = "produtos " + e.Group.Count;
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0) return;

            var index = dataGridView1.SelectedRows[0].Index;
            MarcaDescarcaItem(index);
        }

        private void MarcaDescarcaItem(int index)
        {
            if (dataGridView1.Rows[index].DataBoundItem.GetType() == typeof(GroupRow))
            {
                var grupoItens = (GroupRow) dataGridView1.Rows[index].DataBoundItem;
                foreach (var row in grupoItens.Rows)
                {
                    ((Produto) row).IsOk = grupoItens.CheckrdItens;
                }

            }
            else
            {
                ((Produto)dataGridView1.Rows[index].DataBoundItem).IsOk =
                    !((Produto)dataGridView1.Rows[index].DataBoundItem).IsOk;
            }
            
           
            dataGridView1.Refresh();
        }

        private void dataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        public void AdicionarTodos()
        {

            var progress = 0;
            _produtosSelecionados = new List<Produto>();
            for (int linha = 0; linha < dataGridView1.Rows.Count; linha++)
            {
                if (dataGridView1.Rows[linha].DataBoundItem.GetType() != typeof(GroupRow)) continue;

                var grupoRow = (GroupRow)dataGridView1.Rows[linha].DataBoundItem;

                foreach (Produto produto in grupoRow.Rows)
                {
                    if (!produto.IsOk) continue;

                    produto.produtoId = 0;
                    produto.nome = CapitalizarNome(produto.nome);
                    produto.produtoGuid = Guid.NewGuid().ToString();
                    produto.imagemBase64 = GetImagem();
                    produto.imagem = string.Empty;

                    produto.descricao = string.Empty;

                    _produtosSelecionados.Add(produto);
                }
                
            }


            var categoriasNew = _produtosSelecionados.GroupBy(grop => new { grop.categoriaNome, grop.categoriaId }).Select(sel => new Categoria()
            {
                Descricao = sel.Key.categoriaNome,
                CategoriaId = sel.Key.categoriaId,
                ReferenciaId = sel.Key.categoriaId,
                RestauranteToken = Program.TokenVapVupt
            }).ToList();

            progressBar1.Maximum = categoriasNew.Count + _produtosSelecionados.Count;
            _textInformation = "Iniciando inclusão de categorias";


            foreach (var categoria in categoriasNew)
            {
                var result = new CategoriaService().Adicionar(categoria);
                _textInformation = $"Categorias: {categoria.Descricao} {result}";
                progress++;
                backgroundWorker1.ReportProgress(progress);
            }

            var categoriaService = new CategoriaService();
            var categorias = categoriaService.ObterCategorias(99, 1).ResultToList<Categoria>();

            //var resultado = new ProdutoService().Adicionar(produto);
            Thread.Sleep(1000);

            _textInformation = "Iniciando inclusão de categorias";
            foreach (var produtosSelecionado in _produtosSelecionados)
            {
                var categoria = categorias.FirstOrDefault(w => w.Descricao == produtosSelecionado.categoriaNome);
                if (categoria == null) continue;

                produtosSelecionado.categoriaId = categoria.CategoriaId;
                produtosSelecionado.restauranteId = categoria.RestauranteId;
                produtosSelecionado.situacao = (int)SituacaoCadastroEnum.Ativo;
                produtosSelecionado.tamanhoId = (int)TamanhoEnumView.Grande;
                produtosSelecionado.TokenRestaurante = Program.TokenVapVupt;
                var result = new ProdutoService().Adicionar(produtosSelecionado);

                _textInformation = $"Produto: {produtosSelecionado.nome} {result}";

                progress++;
                backgroundWorker1.ReportProgress(progress);

            }

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            pnlProcesso.Visible = true;
            pnlHeader.Enabled = false;
            pnlAcao.Enabled = false;
            dataGridView1.Enabled = false;

            backgroundWorker1.RunWorkerAsync();
        }

        string CapitalizarNome(string nome)
        {
            string[] excecoes = new string[] { "e", "de", "da", "das", "do", "dos" };
            var palavras = new Queue<string>();
            foreach (var palavra in nome.Split(' '))
            {
                if (!string.IsNullOrEmpty(palavra))
                {
                    var emMinusculo = palavra.ToLower();
                    var letras = emMinusculo.ToCharArray();
                    if (!excecoes.Contains(emMinusculo)) letras[0] = char.ToUpper(letras[0]);
                    palavras.Enqueue(new string(letras));
                }
            }
            return string.Join(" ", palavras);
        }

        private string GetImagem()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    _image.Image.Save(stream, ImageFormat.Png);
                    stream.Close();
                    byte[] imageBytes = stream.ToArray();
                    var base64String = Convert.ToBase64String(imageBytes);
                    return base64String;

                }
            }
            catch (Exception e)
            {
                return String.Empty;

            }

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            AdicionarTodos();
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            label2.Text = _textInformation;
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            pnlProcesso.Visible = false;
            MessageBox.Show("Produtos Cadastrado com sucesso.");
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Space) return;

            if (dataGridView1.SelectedRows.Count <= 0) return;

            var index = dataGridView1.SelectedRows[0].Index;
            MarcaDescarcaItem(index);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (dataGridView1.SelectedRows.Count <= 0) return;

            var index = dataGridView1.SelectedRows[0].Index;
            MarcaDescarcaItem(index);*/
        }
    }
}
