using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Subro.Controls;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FromGrupoProduto : Form
    {
        private readonly ProdutoGrupoService _produtoGrupoService;
        private List<ProdutoGrupo> _produtoGrupos;
        private ProdutoGrupo _produtoGrupo;
        private bool _isAlter;
        public FromGrupoProduto()
        {
            InitializeComponent();
            _produtoGrupoService = new ProdutoGrupoService();
        }

        private void FormTipoOpcoes_Load(object sender, EventArgs e)
        {
            txtDescricao.ReadOnly = true;
            txtNome.ReadOnly = true;
            btnAdicionar.Enabled = false;
            btnRelacionar.Enabled = false;
            btnAddImage.Enabled = false;
            btnAddImageZ.Enabled = false;

            btnNovo.Visible = true;


            CarregaOpcoes();
        }

        void CarregaOpcoes()
        {
            _produtoGrupos = _produtoGrupoService.ObterTodas().ResultToList<ProdutoGrupo>().OrderBy(o => o.Sequencia).ToList();

            var grupoCategorias = new List<RelacaoGrupoCategoriaViewModel>();

            foreach (var produtoOpcaoTipo in _produtoGrupos)
            {
                if (produtoOpcaoTipo.Categorias.Count > 0)
                {
                    var item = produtoOpcaoTipo.Categorias.Select(sel => new RelacaoGrupoCategoriaViewModel()
                    {
                        RestauranteId = sel.RestauranteId,
                        GupoId = produtoOpcaoTipo.GupoId,
                        NomeGrupo = produtoOpcaoTipo.Nome,
                        CategoriaId = sel.CategoriaId,
                        NomeCategoria = sel.Descricao,
                        Imagem = produtoOpcaoTipo.Imagem
                    });

                    grupoCategorias.AddRange(item);
                }
                else
                {
                    var item = new RelacaoGrupoCategoriaViewModel()
                    {
                        RestauranteId = produtoOpcaoTipo.RestauranteId,
                        GupoId = produtoOpcaoTipo.GupoId,
                        NomeGrupo = produtoOpcaoTipo.Nome,
                        CategoriaId = 0,
                        NomeCategoria = "",
                        Imagem = produtoOpcaoTipo.Imagem
                    };

                    grupoCategorias.Add(item);
                }

            }


            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.DataSource = grupoCategorias;

            var grouper = new Subro.Controls.DataGridViewGrouper(dataGridView3);

            grouper.GroupSortOrder = SortOrder.None;
            grouper.Options.StartCollapsed = false;
            grouper.SetGroupOn("NomeGrupo");
            grouper.DisplayGroup += grouper_DisplayGroup;
        }

        void grouper_DisplayGroup(object sender, GroupDisplayEventArgs e)
        {
            e.BackColor = (e.Group.GroupIndex % 2) == 0 ? Color.LightGray : Color.LightSkyBlue;
            e.Header = "";
            e.DisplayValue = e.DisplayValue;
            e.Summary = "" + e.Group.Count;

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtNome.Text))
            {
                Funcoes.Mensagem("É obrigatório informar o nome do grupo", "Validação", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(txtDescricao.Text))
            {
                Funcoes.Mensagem("É obrigatório informar o descrição do grupo", "Validação", MessageBoxButtons.OK);
                return;
            }
            if (!_isAlter)
            {
                var grupo = new ProdutoGrupo()
                {
                    Nome = txtNome.Text,
                    Descricao = txtDescricao.Text,
                    Sequencia = dataGridView3.RowCount + 1,
                    Situacao = 1,
                    Imagem = "",
                    ImagemBase64 = GetImagem(),
                    ImagemZimmer = GetImagem2(),
            };

                _produtoGrupoService.Adicionar(grupo);
                Funcoes.Mensagem("Grupo foi criado com sucesso.", "Informação", MessageBoxButtons.OK);
            }
            else
            {
                _produtoGrupo.Nome = txtNome.Text;
                _produtoGrupo.Descricao = txtDescricao.Text;
                _produtoGrupo.ImagemBase64 = GetImagem();
                _produtoGrupo.ImagemZimmer = GetImagem2();
                _produtoGrupoService.Alterar(_produtoGrupo);
                Funcoes.Mensagem("Grupo foi alterado com sucesso.", "Informação", MessageBoxButtons.OK);
            }



            CarregaOpcoes();

            txtDescricao.Clear();
            txtNome.Clear();

            txtDescricao.ReadOnly = true;
            txtNome.ReadOnly = true;
            btnAdicionar.Enabled = false;
            btnRelacionar.Enabled = false;
            btnAddImage.Enabled = false;
            btnAddImageZ.Enabled = false;

            _isAlter = false;

        }
        private string GetImagem()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    pictureBox1.Image.Save(stream, ImageFormat.Png);
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
        private string GetImagem2()
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    pictureBox2.Image.Save(stream, ImageFormat.Png);
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um grupo para alteração");
                return;
            }

            var index = dataGridView3.SelectedRows[0].Index;

            RelacaoGrupoCategoriaViewModel relacao;
            if (dataGridView3.Rows[index].DataBoundItem.GetType() == typeof(Subro.Controls.GroupRow))
                relacao = (RelacaoGrupoCategoriaViewModel)((Subro.Controls.GroupRow) dataGridView3.Rows[index].DataBoundItem).FirstRow;
            else
                relacao = (RelacaoGrupoCategoriaViewModel)dataGridView3.Rows[index].DataBoundItem;

            var tipo = _produtoGrupos.FirstOrDefault(t => t.GupoId == relacao.GupoId);

            if (index == 0)
            {
                tipo.Sequencia = 1;
                _produtoGrupoService.Alterar(tipo);
                CarregaOpcoes();
                return;
            }


            var indexAlter = index - 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter + 2;

            var relacao2 = (RelacaoGrupoCategoriaViewModel)dataGridView3.Rows[indexAlter].DataBoundItem;
            var tipo2 = _produtoGrupos.FirstOrDefault(t => t.GupoId == relacao2.GupoId);

            tipo.Sequencia = sequenciaNovo;
            _produtoGrupoService.Alterar(tipo);


            tipo2.Sequencia = sequenciaOld;
            _produtoGrupoService.Alterar(tipo2);

            CarregaOpcoes();

            var indexPos = _produtoGrupos.FindIndex(t => t.GupoId == tipo.GupoId);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um grupo para alteração");
                return;
            }

            var index = dataGridView3.SelectedRows[0].Index;

            RelacaoGrupoCategoriaViewModel relacao;
            if (dataGridView3.Rows[index].DataBoundItem.GetType() == typeof(Subro.Controls.GroupRow))
                relacao = (RelacaoGrupoCategoriaViewModel)((Subro.Controls.GroupRow)dataGridView3.Rows[index].DataBoundItem).FirstRow;
            else
                relacao = (RelacaoGrupoCategoriaViewModel)dataGridView3.Rows[index].DataBoundItem;

            var tipo = _produtoGrupos.FirstOrDefault(t => t.GupoId == relacao.GupoId);

            if (index == dataGridView3.RowCount - 1)
            {
                return;
            }


            var indexAlter = index + 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter;

            var relacao2 = (RelacaoGrupoCategoriaViewModel)dataGridView3.Rows[indexAlter].DataBoundItem;
            var tipo2 = _produtoGrupos.FirstOrDefault(t => t.GupoId == relacao2.GupoId);

            tipo.Sequencia = sequenciaNovo;
            _produtoGrupoService.Alterar(tipo);


            tipo2.Sequencia = sequenciaOld;
            _produtoGrupoService.Alterar(tipo2);

            CarregaOpcoes();

            var indexPos = _produtoGrupos.FindIndex(t => t.GupoId == tipo.GupoId);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var index = dataGridView3.SelectedRows[0].Index;
            var relacao = (RelacaoGrupoCategoriaViewModel)dataGridView3.Rows[index].DataBoundItem;
            _produtoGrupo = _produtoGrupos.FirstOrDefault(t => t.GupoId == relacao.GupoId);

            if(_produtoGrupo == null)return;

            txtDescricao.Text = _produtoGrupo.Descricao;
            txtNome.Text = _produtoGrupo.Nome;
            txtLabelGrupo.Text = _produtoGrupo.Nome;

            pictureBox1.Image = null;
            pictureBox2.Image = null;

            if (!string.IsNullOrEmpty(_produtoGrupo.Imagem))
            {
                try
                {
                    pictureBox1.Load($"{_produtoGrupo.Imagem}");
                }
                catch
                {
                   //Ignore
                }
            }

            if (!string.IsNullOrEmpty(_produtoGrupo.ImagemZimmer))
            {
                try
                {
                    pictureBox2.Load($"{_produtoGrupo.ImagemZimmer}");
                }
                catch
                {
                    //Ignore
                }
            }


            _isAlter = true;
            txtDescricao.ReadOnly = false;
            txtNome.ReadOnly = false;
            btnAdicionar.Enabled = true;
            btnExcluir.Enabled = true;
            btnAddImage.Enabled = true;
            btnAddImageZ.Enabled = true;
            btnRelacionar.Enabled = true;
            btnExcluir.Enabled = true;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            label2.Text = "";
            txtLabelGrupo.Text = "";
            txtDescricao.Clear();
            txtNome.Clear();
            txtDescricao.ReadOnly = false;
            txtNome.ReadOnly = false;
            btnAddImage.Visible = true;
            btnAddImageZ.Visible = true;
            btnRelacionar.Enabled = false;
            btnExcluir.Enabled = false;
            _isAlter = false;

            _produtoGrupo = new ProdutoGrupo();
            btnAdicionar.Enabled = true;
            txtNome.Focus();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Deseja realmente excluir o grupo{_produtoGrupo.Descricao}?", 
                "Cadastro de Produto", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes && _produtoGrupo != null)
            {
                return;
            }
            else
            {
                _produtoGrupoService.Excluir(_produtoGrupo);
                MessageBox.Show("grupo excluido com sucesso");

                CarregaOpcoes();
            }
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            try
            {
                var qualidades = new int[4] { 60, 40, 20, 15 };

                var fileDialog = Funcoes.BuscarImagem();
                var arquivo = fileDialog.FileName;


                if (string.IsNullOrEmpty(arquivo)) return;

                var fileInfo = new FileInfo(arquivo);
                var arquivoRezise = $"{fileDialog.FileName.Replace(fileDialog.SafeFileName, "")}res_{fileDialog.SafeFileName}";


                if ((fileInfo.Length / 1024) > 100) //Verifica se arquivo tem mais que 100kb
                {

                    foreach (var qualidade in qualidades)
                    {

                        if ((fileInfo.Length / 1024) > 100)
                        {
                            if (File.Exists(arquivoRezise))
                                File.Delete(arquivoRezise);

                            Funcoes.reduzir(arquivo, arquivoRezise, qualidade);
                            fileInfo = new FileInfo(arquivoRezise);
                        }
                        else
                        {
                            break;
                        }
                    }


                    arquivo = arquivoRezise;
                }


                var imagem = Image.FromFile(arquivo);

                pictureBox1.Image = imagem;
            }
            catch (Exception exception)
            {
                Funcoes.Mensagem("Erro ao abrir a imagem", "Gestor", MessageBoxButtons.OK);
            }
        }

        private void btnRelacionar_Click(object sender, EventArgs e)
        {

            using (var form = new FormRelacaoGrupoCategoria(_produtoGrupo))
            {
                form.ShowDialog();
                CarregaOpcoes();
            }
        }

        private void txtNome_Leave(object sender, EventArgs e)
        {
            txtLabelGrupo.Text = txtNome.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var qualidades = new int[4] { 60, 40, 20, 15 };

                var fileDialog = Funcoes.BuscarImagem();
                var arquivo = fileDialog.FileName;


                if (string.IsNullOrEmpty(arquivo)) return;

                var fileInfo = new FileInfo(arquivo);
                var arquivoRezise = $"{fileDialog.FileName.Replace(fileDialog.SafeFileName, "")}res_{fileDialog.SafeFileName}";


                if ((fileInfo.Length / 1024) > 100) //Verifica se arquivo tem mais que 100kb
                {

                    foreach (var qualidade in qualidades)
                    {

                        if ((fileInfo.Length / 1024) > 100)
                        {
                            if (File.Exists(arquivoRezise))
                                File.Delete(arquivoRezise);

                            Funcoes.reduzir(arquivo, arquivoRezise, qualidade);
                            fileInfo = new FileInfo(arquivoRezise);
                        }
                        else
                        {
                            break;
                        }
                    }


                    arquivo = arquivoRezise;
                }


                var imagem = Image.FromFile(arquivo);

                pictureBox2.Image = imagem;

            }
            catch (Exception exception)
            {
                Funcoes.Mensagem("Erro ao abrir a imagem", "Gestor", MessageBoxButtons.OK);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
