using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Subro.Controls;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormCadastroProdutos : Form
    {
        private readonly ProdutoOpcaoService _produtoOpcaoService;
        private ProdutoShiftService _produtoShiftService;
        private List<ProdutoShifts> _produtosShifts;
        private List<Produto> _produtos;
        private List<Produto> _produtosBase;
        private Produto _produto;
        private Produto _produtoCopia;
        private int _totalPage;
        private int _referenciaId;
        public FormCadastroProdutos()
        {
            InitializeComponent();
            _produtos = new List<Produto>();
            _produtoOpcaoService = new ProdutoOpcaoService();
            _produtoShiftService = new ProdutoShiftService();
            Load += FormCadastroProdutos_Load;
        }

        private void FormCadastroProdutos_Load(object sender, EventArgs e)
        {

            txtProdutoReferencia.Enabled = Program.TipoDatabase == TipoDatabaseEnum.SQLSever;
            CarregaProdutoApi();

            CarregaCategoria();

            cbTamaho.DataSource = Funcoes.Listar(typeof(TamanhoEnumView));
            cbTamaho.DisplayMember = "Value";
            cbTamaho.ValueMember = "Key";
            cbTamaho.SelectedIndex = -1;

            cbSituacao.DataSource = Funcoes.Listar(typeof(SituacaoCadastroEnum));
            cbSituacao.DisplayMember = "Value";
            cbSituacao.ValueMember = "Key";
            cbSituacao.SelectedIndex = -1;

            chkIsControlstock.Checked = true;
            chkDisponibilidade.Checked = false;

            dtpStartTime.Enabled = false;
            dtpEndTime.Enabled = false;
            chkMonday.Enabled = false;
            chkTuesday.Enabled = false;
            chkWednesday.Enabled = false;
            chkThursday.Enabled = false;
            chkFriday.Enabled = false;
            chkSaturday.Enabled = false;
            chkSunday.Enabled = false;

        }

        private void CarregaProdutoApi()
        {
            var rootProdutos = new ProdutoService().ObterProdutos(9999, 1);

            _totalPage = rootProdutos.totalPage;
            _produtosBase = rootProdutos.results.ToList<Produto>().OrderBy(t => t.categoriaNome).ThenBy(t => t.sequencia)
                .ToList();
            _produtos = _produtosBase;

            CarregaGridProdutos();
        }

        private void CarregaGridProdutos()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = _produtos;

            var grouper = new Subro.Controls.DataGridViewGrouper(dataGridView1);

            grouper.GroupSortOrder = SortOrder.None;
            grouper.Options.StartCollapsed = false;
            grouper.SetGroupOn("categoriaNome");
            grouper.DisplayGroup += grouperProdutos_DisplayGroup;
        }

        void grouperProdutos_DisplayGroup(object sender, GroupDisplayEventArgs e)
        {
            e.BackColor = (e.Group.GroupIndex % 2) == 0 ? Color.LightGray : Color.LightSkyBlue;
            e.Header = "";
            e.DisplayValue = e.DisplayValue;
            e.Summary = "" + e.Group.Count;

        }

        private void CarregaCategoria()
        {
            var categoriaService = new CategoriaService();
            var rootCategorias = categoriaService.ObterCategorias(99, 1).ResultToList<Categoria>();

            cbCategoria.DataSource = rootCategorias.Where(t => t.Situacao == SituacaoCadastro.Ativo).ToList();
            cbCategoria.DisplayMember = "Descricao";
            cbCategoria.ValueMember = "CategoriaId";
            cbCategoria.SelectedIndex = -1;
            cbCategoria.SelectedValueChanged += CbCategoria_SelectedValueChanged;
        }

        private void CbCategoria_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbCategoria.SelectedIndex == -1) return;

            CarregaComplementos();

        }

        private void btnInsertCategoria_Click(object sender, EventArgs e)
        {
            /*
            btnInsertCategoria.Visible = false;
            cbCategoria.Visible = false;

            txtInserCategoria.Visible = true;
            txtInserCategoria.Focus();
            */
            using (var form = new FormCategoria())
            {
                form.ShowDialog();
                CarregaCategoria();
            }
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (string.IsNullOrEmpty(txtProdutoReferencia.Text)) return;

            using (var form = new FormPesquisaProduto(txtProdutoReferencia.Text))
            {
                form.ShowDialog();
                if (form.Produto == null) return;

                var produtoRelacao = form.Produto;

                txtProdutoReferencia.Text = produtoRelacao.nome;
                txtDescricaoApp.Text = ToTitleCase(produtoRelacao.nome);
                txtValorVenda.Text = produtoRelacao.valorVenda.ToString("N2");
                // txtCategoria.Text = ToTitleCase(produtoRelacao.categoriaNome);

                _produto.referenciaId = produtoRelacao.referenciaId;

                if (_produto != null) return;

                //_produto = new Produto();
                _produto.nome = ToTitleCase(produtoRelacao.nome);
                _produto.valorVenda = produtoRelacao.valorVenda;
                _produto.categoriaNome = ToTitleCase(produtoRelacao.categoriaNome);
            }

        }

        private string ToTitleCase(string texto)
        {

            return string.Concat(char.ToUpper(texto[0]), texto.Substring(1).ToLower());
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            if (dataGridView1.Rows[e.RowIndex].DataBoundItem.GetType() != typeof(Produto)) return;
            var produto = (Produto)dataGridView1.Rows[e.RowIndex].DataBoundItem;

            if (e.ColumnIndex == 6)
            {
                _produto = produto;
                CarregaProduto();
            }
            else if (e.ColumnIndex == 7)
            {
                var result = MessageBox.Show($"Deseja realmente pausar o produto {produto.nome}?", "Cadastro de Produto", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                var produtoService = new ProdutoService();
                produto.situacao = produto.situacao == 1 ? 2 : 1;
                produtoService.Adicionar(produto);

                CarregaGridProdutos();


                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }



        }

        private void dgvVenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Funcoes.Val_NumeroKey(e.KeyChar.ToString()))
                e.Handled = false;
            else
            {
                e.Handled = true;
                SendKeys.Send("{Enter}");
            }

        }
        private void dgvVenda_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var tb = (DataGridViewTextBoxEditingControl)e.Control;
            tb.KeyPress += new KeyPressEventHandler(dgvVenda_KeyPress);

            //e.Control.KeyPress += new KeyPressEventHandler(dgvVenda_KeyPress);
        }

        private void CarregaProduto()
        {
            LimparForm();


            if (_produto == null) return;

            if (Program.TipoDatabase == TipoDatabaseEnum.SQLSever)
            {
                if (_produto.referenciaId > 0)
                {
                    var produtoRelacao = new ProdutoRepository().ObterPorNomeOrId("", _produto.referenciaId)
                        .FirstOrDefault();
                    if (produtoRelacao != null)
                    {
                        _referenciaId = produtoRelacao.referenciaId;
                        txtProdutoReferencia.Text = produtoRelacao.nome;
                    }
                }


            }
            else
            {
                _referenciaId = _produto.referenciaId;
                txtProdutoReferencia.Text = _produto.nome;
            }

            txtDescricaoApp.Text = _produto.nome;
            txtValorVenda.Text = _produto.valorVenda.ToString("N2");
            txtVlrRegular.Text = _produto.valorRegular.ToString("N2");
            txtVlrPromocao.Text = _produto.valorPromocao.ToString("N2");
            txtDescricao.Text = _produto.descricao;
            pictureBox1.ImageLocation = _produto.imagem;
            cbCategoria.SelectedValue = _produto.categoriaId;
            cbTamaho.SelectedValue = _produto.tamanhoId;
            cbSituacao.SelectedValue = _produto.situacao;
            chkPartMeioMeio.Checked = _produto.isPartMeioMeio;
            txtQtdeFracao.ValueNumeric = _produto.numberMeiomeio;
            tabControl1.SelectedTab = tabPage1;
            btnCopiaCadastro.Enabled = true;
            tabControl2.SelectedTab = tabDetalhe;
            chkIsControlstock.Checked = _produto.isControlstock;
            txtEstoque.ValueNumeric = _produto.stock;



        }

        private void LimparForm()
        {
            _referenciaId = 0;
            txtProdutoReferencia.Clear();
            // txtCategoria.Clear();
            txtDescricaoApp.Clear();
            txtValorVenda.Clear();
            txtVlrRegular.Clear();
            txtVlrPromocao.Clear();
            txtDescricao.Clear();
            pictureBox1.ImageLocation = null;
            cbCategoria.SelectedIndex = -1;
            cbTamaho.SelectedIndex = -1;
            cbSituacao.SelectedIndex = -1;
            chkPartMeioMeio.Checked = false;
            txtQtdeFracao.ValueNumeric = 0;
            chkDisponibilidade.Checked = false;
            chkIsControlstock.Checked = false;
            txtEstoque.ValueNumeric = 0;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LimparForm();
            toolStripButton3.Enabled = true;
            cbTamaho.SelectedIndex = 0;
            pictureBox1.Load("http://zipsoftware2.ddns.com.br:56435/api/galeria/produtos/foto_em_breve.png");

            _produto = new Produto();
            _produto.produtoGuid = Guid.NewGuid().ToString();
            _produto.tamanhoId = (int)TamanhoEnumView.Grande;
            _produto.situacao = (int)SituacaoCadastroEnum.Ativo;
            _produto.isControlstock = false;

            if (Program.TipoDatabase == TipoDatabaseEnum.SQLite)
                _produto.referenciaId = _produtos.Max(t => t.referenciaId) + 1;

            CarregaProduto();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            var search = $"{textBox1.Text.ToUpper()}";

            _produtos = !Funcoes.Val_NumeroKey(search)
                ? _produtosBase.Where(p => p.nome.ToUpper().Contains(search)).ToList()
                : _produtosBase.Where(p => p.referenciaId.ToString().Contains(search)).ToList();

            CarregaGridProdutos();
        }

        private void txtInserCategoria_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;


        }


        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            _produto.referenciaId = _produto.referenciaId > 0 ? _produto.referenciaId : _referenciaId;
            _produto.TokenRestaurante = Program.TokenVapVupt;
            _produto.nome = txtDescricaoApp.Text;
            _produto.descricao = txtDescricao.Text;
            _produto.valorVenda = txtValorVenda.ValueNumeric;
            _produto.valorRegular = txtVlrRegular.ValueNumeric;
            _produto.valorPromocao = txtVlrPromocao.ValueNumeric;
            _produto.categoriaId = (int)cbCategoria.SelectedValue;
            _produto.tamanhoId = (int)cbTamaho.SelectedValue;
            _produto.situacao = (int)cbSituacao.SelectedValue;
            _produto.produtoGuid = _produto.produtoGuid ?? Guid.NewGuid().ToString();
            _produto.imagem = _produto.imagem ?? "";
            _produto.isPartMeioMeio = chkPartMeioMeio.Checked;
            _produto.numberMeiomeio = !chkPartMeioMeio.Checked ? (int)txtQtdeFracao.ValueNumeric : 0;

            _produto.isControlstock = chkIsControlstock.Checked;
            _produto.stock = int.Parse(txtEstoque.ValueNumeric.ToString());

            //_produto.categoriaNome = "";
            _produto.imagemBase64 = GetImagem();

            var resultado = new ProdutoService().Adicionar(_produto);

            MessageBox.Show(resultado);

            CarregaProdutoApi();

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
        private Bitmap ResizeImage(Image image, int width, int height)
        {

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;

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

                            reduzir(arquivo, arquivoRezise, qualidade);
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
            catch
            {
                Funcoes.Mensagem("Erro ao abrir a imagem", "Gestor", MessageBoxButtons.OK);
            }

        }

        private void btnAddComplemento_Click(object sender, EventArgs e)
        {
            /*if (cbCategoria.SelectedIndex == -1) return;

            var grupoId = (int)cbCategoria.SelectedValue;

            var complemento = new Complemento()
            {
                CategoriaId = grupoId,
                Descricao = txtComplementos.Text,
                Valor = txtValorComplemento.ValueNumeric
            };

            var resultado = new ProdutoService().AddComplementos(complemento);

            MessageBox.Show(resultado);

            txtComplementos.Clear();
            txtValorComplemento.Clear();

            CarregaComplementos();*/

            if (cbOpcoesTipo.SelectedIndex == -1)
            {
                Funcoes.Mensagem("É obrigatório informar o tipo de complementos", "Validação", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(txtComplementos.Text))
            {
                Funcoes.Mensagem("É obrigatório informar o nome do complemento", "Validação", MessageBoxButtons.OK);
                return;
            }

            var result = MessageBox.Show("Item disponível a todos os produtos?", "Complementares",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes;

            var complementar = new ProdutoOpcao()
            {
                Nome = txtComplementos.Text,
                ProdutoId = _produto.produtoId,
                ProdutoPdv = txtProdutoPdv.Text,
                ProdutosOpcaoTipoId = (int)cbOpcoesTipo.SelectedValue,
                Valor = txtValorComplemento.ValueNumeric,
                Sequencia = 999,
                Replicar = result
            };



            _produtoOpcaoService.Adicionar(complementar);
            CarregaComplementares();
            cbOpcoesTipo.SelectedIndex = -1;
            txtComplementos.Clear();
            txtValorComplemento.ValueNumeric = 0;
            txtProdutoPdv.Clear();


        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (dataGridView1.SelectedRows.Count == 0) return;

            var index = dataGridView1.SelectedRows[0].Index;

            _produto = _produtos[index];
            CarregaProduto();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            if (dataGridView2.SelectedCells[0].ColumnIndex != 2) return;

            var complemento = (Complemento)dataGridView2.Rows[e.RowIndex].DataBoundItem;
            complemento.TokenRestaurante = Program.TokenVapVupt;
            if (MessageBox.Show($"Confirma exclusão do complemento\n[{complemento.Descricao}]?",
                    "Exclusão de complemento", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No) return;

            var produtoService = new ProdutoService();

            try
            {
                var result = produtoService.RemoveComplementos(complemento);
                MessageBox.Show(result);

                CarregaComplementos();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw;
            }
            */
            var index = e.RowIndex;
            if (index == -1) return;

            var data = (ProdutoOpcao)dataGridView2.Rows[index].DataBoundItem;
            if (e.ColumnIndex == 2)
            {
                data.Situacao = data.Situacao == 0 ? 1 : 0;
                _produtoOpcaoService.Alterar(data);
                CarregaComplementares();
                //Altera

            }
            else if (e.ColumnIndex == 3)
            {
                if (MessageBox.Show($"Confirma exclusão do complemento\n[{data.Nome}]?",
                        "Exclusão de complemento", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.No) return;

                try
                {
                    var result = _produtoOpcaoService.ExcluirRelacao(data.ProdutosOpcaoId, _produto.produtoId);
                    MessageBox.Show(result);

                    CarregaComplementares();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);

                }
            }




        }

        private void CarregaComplementos()
        {
            /*
            var grupoId = (int)cbCategoria.SelectedValue;
            var complementos = new ProdutoService().ObterComplementos(grupoId);
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = complementos;*/
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0) return;

            var produto = (Produto)dataGridView1.Rows[e.RowIndex].DataBoundItem;

            switch (produto.situacao)
            {
                case 1:
                    e.CellStyle.BackColor = Color.White;
                    break;
                case 2:
                    e.CellStyle.BackColor = Color.Gainsboro;
                    break;
                case 3:
                    e.CellStyle.BackColor = Color.AntiqueWhite;
                    break;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            _produto.referenciaId = _referenciaId > 0 ? _referenciaId : _produto.referenciaId;
            _produto.TokenRestaurante = Program.TokenVapVupt;
            _produto.nome = txtDescricaoApp.Text;


            if (MessageBox.Show($"Confirma exclusão do produto\n[{_produto.nome}]?",
                    "Exclusão de produto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No) return;

            try
            {
                var resultado = new ProdutoService().Excluir(_produto);
                MessageBox.Show(resultado);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

            }


            LimparForm();

            CarregaProdutoApi();
            tabControl1.SelectedTab = tabPage2;

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedTab != tabPage1) return;

            var produtoOk = _produto?.produtoId != null;
            toolStripButton3.Enabled = produtoOk;
            toolStripButton2.Enabled = produtoOk;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void reduzir(string caminhoArquivoOriginal, string caminhoArquivoDestino, long qualidade)
        {
            Bitmap myBitmap;
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Create a Bitmap object based on a BMP file.
            myBitmap = new Bitmap(caminhoArquivoOriginal);

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID

            // for the Quality parameter category.
            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file with quality level 25.            
            myEncoderParameter = new EncoderParameter(myEncoder, qualidade);
            myEncoderParameters.Param[0] = myEncoderParameter;


            myBitmap.Save(caminhoArquivoDestino, myImageCodecInfo, myEncoderParameters);
            myBitmap.Dispose();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void lkAddCategoria_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var form = new FormTipoOpcoes())
            {
                form.ShowDialog();

                CarregaComplementares();
            }
        }

        private void tabControl2_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl2.SelectedTab == tabComplementares && _produto != null)
            {
                var data = _produtoOpcaoService.ObterByTipos().ResultToList<ProdutoOpcaoTipo>();

                cbOpcoesTipo.DataSource = data;
                cbOpcoesTipo.DisplayMember = "Nome";
                cbOpcoesTipo.ValueMember = "ProdutosOpcaoTipoId";
                cbOpcoesTipo.SelectedIndex = -1;

                CarregaComplementares();
            }
            else if (tabControl2.SelectedTab == tabProdShift && _produto != null)
            {
                carregaProdShifts();
            }
        }


        void carregaProdShifts()
        {
            var _produtosShifts = _produtoShiftService.ObterTodas(_produto.produtoId).ToList();
            dataGridView4.AutoGenerateColumns = false;
            dataGridView4.DataSource = _produtosShifts;
        }
        void CarregaComplementares()
        {
            var produtoOpcoes = _produtoOpcaoService.ObterByProdutoId(_produto.produtoId.ToString()).ResultToList<ProdutoOpcaoTipo>();

            var produtoTipo = new List<ProdutoOpcao>();

            foreach (var produtoOpcaoTipo in produtoOpcoes.OrderBy(t => t.Sequencia))
            {
                var item = produtoOpcaoTipo.ProdutoOpcaos.Select(sel => new ProdutoOpcao()
                {
                    RestauranteId = sel.RestauranteId,
                    ProdutoId = sel.ProdutoId,
                    Sequencia = sel.Sequencia,
                    Nome = sel.Nome,
                    ProdutosOpcaoTipoId = sel.ProdutosOpcaoTipoId,
                    ProdutoPdv = sel.ProdutoPdv,
                    ProdutosOpcaoId = sel.ProdutosOpcaoId,
                    Valor = sel.Valor,
                    TipoNome = produtoOpcaoTipo.Nome,
                    Situacao = sel.Situacao
                });

                produtoTipo.AddRange(item);
            }


            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = produtoTipo;

            var grouper = new Subro.Controls.DataGridViewGrouper(dataGridView2);

            grouper.GroupSortOrder = SortOrder.None;
            grouper.Options.StartCollapsed = false;
            grouper.SetGroupOn("TipoNome");
            grouper.DisplayGroup += grouper_DisplayGroup;
        }

        void grouper_DisplayGroup(object sender, GroupDisplayEventArgs e)
        {
            e.BackColor = (e.Group.GroupIndex % 2) == 0 ? Color.LightGray : Color.LightSkyBlue;
            e.Header = "";
            e.DisplayValue = e.DisplayValue;
            e.Summary = "" + e.Group.Count;

        }

        private void dataGridView2_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            var index = e.Row.Index;
            if (index == -1) return;
            if (dataGridView2.Rows[index].DataBoundItem.GetType() != typeof(ProdutoOpcao)) return;


            var data = (ProdutoOpcao)dataGridView2.Rows[index].DataBoundItem;

            dataGridView2.Rows[index].Cells[2].Value = data.Situacao == 1 ? Properties.Resources.pause_16 : Properties.Resources.play_16;
            dataGridView2.Rows[index].DefaultCellStyle.BackColor = data.Situacao == 1 ? Color.White : Color.LightCoral;
        }

        private void btnCopiaCadastro_Click(object sender, EventArgs e)
        {
            if (_produto == null) return;

            try
            {
                var resultado = new ProdutoService().Copiar(_produto);
                _produto = resultado;

                CarregaProduto();

                CarregaProdutoApi();
            }
            catch (Exception exception)
            {
                Funcoes.Mensagem(exception.Message, "Erro", MessageBoxButtons.OK);

            }

        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var itemSource = (ProdutoOpcao)dataGridView2.Rows[e.RowIndex].DataBoundItem;
            _produtoOpcaoService.Alterar(itemSource);
            CarregaComplementares();

        }

        private void cbOpcoesTipo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbOpcoesTipo.SelectedIndex == -1) return;

            var item = (ProdutoOpcaoTipo)cbOpcoesTipo.SelectedItem;
            btnItensOpcoes.Enabled = item.ProdutoOpcaos.Any();

        }

        private void btnItensOpcoes_Click(object sender, EventArgs e)
        {
            if (cbOpcoesTipo.SelectedIndex == -1) return;

            if (MessageBox.Show("Deseja carregar todos os complementares ja existente?", "Carregar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            var item = (ProdutoOpcaoTipo)cbOpcoesTipo.SelectedItem;

            foreach (var itemProdutoOpcao in item.ProdutoOpcaos)
            {
                if (!itemProdutoOpcao.Replicar) continue;

                var relacao = new ProdutosOpcaoTipoRelacao()
                {
                    ProdutoId = _produto.produtoId,
                    ProdutosOpcaoId = itemProdutoOpcao.ProdutosOpcaoId,
                };

                _produtoOpcaoService.Relacionar(relacao);
            }

            CarregaComplementares();
        }

        private void excluirTodosDoMesmoTipoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0) return;

            var itemSelected = dataGridView2.SelectedRows[0].DataBoundItem;
            if (itemSelected.GetType() != typeof(GroupRow))
            {
                MessageBox.Show("Selecione o grupo de complementares que deseja excluir", "Exclusão", MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show("Confirma a exclusão dos itens relacionados a esse tipo de complementar?", "Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            var data = (GroupRow)itemSelected;

            foreach (var ownerItem in data.Rows)
            {
                if (ownerItem.GetType() != typeof(ProdutoOpcao)) return;

                var produtoOpcao = (ProdutoOpcao)ownerItem;

                _produtoOpcaoService.ExcluirRelacao(produtoOpcao.ProdutosOpcaoId, _produto.produtoId);
            }

            CarregaComplementares();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2 && e.ColumnIndex != 3 && e.ColumnIndex != 4 && e.ColumnIndex != 5) return;

            try
            {
                if (dataGridView1.Rows[e.RowIndex].DataBoundItem.GetType() != typeof(Produto)) return;

                var produto = (Produto)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                if (produto.stock > 0)
                    produto.isControlstock = true;

                var produtoService = new ProdutoService();

                produto.TokenRestaurante = Program.TokenVapVupt;
                produtoService.Adicionar(produto);
            }
            catch (Exception exception)
            {
                Funcoes.Mensagem(exception.Message, "Ops... ocorreu um erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            try
            {
                var index = e.Row.Index;
                if (index == -1) return;
                if (dataGridView1.Rows[index].DataBoundItem.GetType() != typeof(Produto)) return;


                var data = (Produto)dataGridView1.Rows[index].DataBoundItem;

                dataGridView1.Rows[index].Cells[7].Value = data.situacao == 1 ? Properties.Resources.pause_16 : Properties.Resources.play_16;
                dataGridView1.Rows[index].DefaultCellStyle.BackColor = data.situacao == 1 ? Color.White : Color.LightCoral;

                if (data.situacao != 1) return;

                dataGridView1.Rows[index].Cells[5].Style.BackColor = (data.isControlstock && data.stock == 0)
                    ? Color.LightCoral
                    : Color.White;

                //dataGridView1.Refresh();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);

            }

        }

        private void chkDispSempre_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ManageCheckGroupBox(CheckBox chk, GroupBox grp)
        {
            // Make sure the CheckBox isn't in the GroupBox.
            // This will only happen the first time.
            if (chk.Parent == grp)
            {
                // Reparent the CheckBox so it's not in the GroupBox.
                grp.Parent.Controls.Add(chk);

                // Adjust the CheckBox's location.
                chk.Location = new Point(
                    chk.Left + grp.Left,
                    chk.Top + grp.Top);

                // Move the CheckBox to the top of the stacking order.
                chk.BringToFront();
            }

            // Enable or disable the GroupBox.
            grp.Enabled = chk.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ManageCheckGroupBox(chkIsControlstock, gbEstoque);
        }

        private void chkDisponibilidade_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormCadastroProdutos_Load_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gbDisponibilidade_Enter(object sender, EventArgs e)
        {

        }

        private void chkDispQuinta_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {

            {
                if ((chkMonday.Checked || chkTuesday.Checked || chkWednesday.Checked ||
                    chkThursday.Checked || chkFriday.Checked || chkSaturday.Checked || chkSunday.Checked) == false)
                {
                    Funcoes.Mensagem("Preencha a disponibilidade do Produto", "Informação", MessageBoxButtons.OK);
                }
                else
                {
                    var produtoShift = new ProdutoShifts()
                    {
                        StartTime = dtpStartTime.Value,
                        EndTime = dtpEndTime.Value,
                        Monday = chkMonday.Checked,
                        Tuesday = chkTuesday.Checked,
                        Wednesday = chkWednesday.Checked,
                        Thursday = chkThursday.Checked,
                        Friday = chkFriday.Checked,
                        Saturday = chkSaturday.Checked,
                        Sunday = chkSunday.Checked,
                        ProdutoId = _produto.produtoId,

                    };

                    _produtoShiftService.Adicionar(produtoShift);
                    Funcoes.Mensagem("Horário de funcionamento foi criado com sucesso.", "Informação", MessageBoxButtons.OK);
                    carregaProdShifts();
                }

            }
        }

        private void chkDisponibilidade_CheckedChanged_1(object sender, EventArgs e)
        {

            if (chkDisponibilidade.Checked == true)
            {
                dtpStartTime.Enabled = true;
                dtpEndTime.Enabled = true;
                chkMonday.Enabled = true;
                chkTuesday.Enabled = true;
                chkWednesday.Enabled = true;
                chkThursday.Enabled = true;
                chkFriday.Enabled = true;
                chkSaturday.Enabled = true;
                chkSunday.Enabled = true;
            }
            else
            {
                dtpStartTime.Enabled = false;
                dtpEndTime.Enabled = false;
                chkMonday.Enabled = false;
                chkTuesday.Enabled = false;
                chkWednesday.Enabled = false;
                chkThursday.Enabled = false;
                chkFriday.Enabled = false;
                chkSaturday.Enabled = false;
                chkSunday.Enabled = false;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 9) return;

            var abertura = (ProdutoShifts)dataGridView4.Rows[e.RowIndex].DataBoundItem;
            var result = MessageBox.Show($"Deseja realmente excluir a disponibilidade selecionada?",
            "Cadastro de Produto", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result != DialogResult.Yes && abertura != null)
            {
                return;
            }
            else
            {
                _produtoShiftService.Excluir(abertura);
                Funcoes.Mensagem("Horário de funcionamento foi excluido com sucesso.", "Informação", MessageBoxButtons.OK);
                carregaProdShifts();
            }
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            if (_produto == null)
            {
                Funcoes.Mensagem("Selecione um produto para alterar as informações desta aba.", "Informação", MessageBoxButtons.OK);
                tabControl1.SelectedTab = tabPage2;
            }
            else
            {
                return;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {

            var indexGrupo = dataGridView2.SelectedRows[0].Index;

            var tipo = (ProdutoOpcao)dataGridView2.Rows[indexGrupo].DataBoundItem;

            var groupp = (GroupingSource)dataGridView2.DataSource;

            var produtoOpcoesIndex = (List<ProdutoOpcao>)groupp.DataSource;//.Where(t => t.GetType() != typeof(GroupRow));
            //(List<ProdutoOpcao>)
            var index = produtoOpcoesIndex.FindIndex(t => t.ProdutosOpcaoId == tipo.ProdutosOpcaoId);

            if (index == 0)
            {
                tipo.Sequencia = 1;
                _produtoOpcaoService.Alterar(tipo);
                CarregaComplementares();
                return;
            }


            var indexAlter = index - 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter + 2;

            var tipo2 = (ProdutoOpcao)dataGridView2.Rows[indexGrupo-1].DataBoundItem;

            tipo.Sequencia = sequenciaNovo;
            _produtoOpcaoService.Alterar(tipo);


            tipo2.Sequencia = sequenciaOld;
            _produtoOpcaoService.Alterar(tipo2);

            CarregaComplementares();

            var group = (List<ProdutoOpcao>)((GroupingSource)dataGridView2.DataSource).List;

            //var produtoOpcoes = (List<ProdutoOpcao>)dataGridView1.DataSource;

            var indexPos = group.FindIndex(t => t.ProdutosOpcaoId == tipo.ProdutosOpcaoId);
            dataGridView2.Rows[1].Selected = false;
            dataGridView2.Rows[indexPos].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var indexGrupo = dataGridView2.SelectedRows[0].Index;
            
            var tipo = (ProdutoOpcao)dataGridView2.Rows[indexGrupo].DataBoundItem;

            var groupp = (GroupingSource)dataGridView2.DataSource;

            var produtoOpcoesIndex = (List<ProdutoOpcao>)groupp.DataSource;//.Where(t => t.GetType() != typeof(GroupRow));
            //(List<ProdutoOpcao>)
            var index = produtoOpcoesIndex.FindIndex(t => t.ProdutosOpcaoId == tipo.ProdutosOpcaoId);

            if (index == dataGridView2.RowCount - 1)
            {
                return;
            }


            var indexAlter = index + 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter;

            var tipo2 = (ProdutoOpcao)dataGridView2.Rows[indexGrupo+1].DataBoundItem;

            tipo.Sequencia = sequenciaNovo;
            _produtoOpcaoService.Alterar(tipo);


            tipo2.Sequencia = sequenciaOld;
            _produtoOpcaoService.Alterar(tipo2);

            CarregaComplementares();

            var group = (GroupingSource)dataGridView2.DataSource;

            var produtoOpcoes = (List<ProdutoOpcao>)group.DataSource;//.Where(t => t.GetType() != typeof(GroupRow));
            //(List<ProdutoOpcao>)
            //var indexPos = produtoOpcoes.FindIndex(t => t.ProdutosOpcaoId == tipo.ProdutosOpcaoId);
            //dataGridView2.Rows[indexGrupo+1].Selected = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var produtoService = new ProdutoService();
            var indexGrupo = dataGridView1.SelectedRows[0].Index;

            var tipo = (Produto)dataGridView1.Rows[indexGrupo].DataBoundItem;

            var podutopp = (GroupingSource)dataGridView1.DataSource;

            var produtoIndex = (List<Produto>)podutopp.DataSource;//.Where(t => t.GetType() != typeof(GroupRow));
            //(List<ProdutoOpcao>)
            var index = produtoIndex.FindIndex(t => t.produtoGuid == tipo.produtoGuid);

            if (index == dataGridView2.RowCount - 1)
            {
                return;
            }


            var indexAlter = index + 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter;

            var tipo2 = (Produto)dataGridView1.Rows[indexGrupo + 1].DataBoundItem;

            tipo.sequencia = sequenciaNovo;
            tipo.TokenRestaurante = Program.TokenVapVupt;
            produtoService.Adicionar(tipo);


            tipo2.sequencia = sequenciaOld;
            tipo2.TokenRestaurante = Program.TokenVapVupt;
            produtoService.Adicionar(tipo2);

            //CarregaComplementares();

            //var group = (GroupingSource)dataGridView1.DataSource;



            CarregaProdutoApi();

            var group = (List<Produto>)((GroupingSource)dataGridView1.DataSource).List;
            var indexPos = group.FindIndex(t => t.produtoId == tipo.produtoId);
            dataGridView1.Rows[1].Selected = false;
            dataGridView1.Rows[indexPos].Selected = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var produtoService = new ProdutoService();
            var indexGrupo = dataGridView1.SelectedRows[0].Index;

            var tipo = (Produto)dataGridView1.Rows[indexGrupo].DataBoundItem;

            var groupp = (GroupingSource)dataGridView1.DataSource;

            var produtoOpcoesIndex = (List<Produto>)groupp.DataSource;//.Where(t => t.GetType() != typeof(GroupRow));
            //(List<ProdutoOpcao>)
            var index = produtoOpcoesIndex.FindIndex(t => t.produtoId == tipo.produtoId);

            if (index == 0)
            {
                tipo.sequencia = 1;
                produtoService.Adicionar(tipo);
                CarregaComplementares();
                return;
            }


            var indexAlter = index - 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter + 2;

            var tipo2 = (Produto)dataGridView1.Rows[indexGrupo - 1].DataBoundItem;

            tipo.sequencia = sequenciaNovo;
            tipo.TokenRestaurante = Program.TokenVapVupt;
            produtoService.Adicionar(tipo);


            tipo2.sequencia = sequenciaOld;
            tipo2.TokenRestaurante = Program.TokenVapVupt;
            produtoService.Adicionar(tipo2);


            var group = (List<Produto>)((GroupingSource)dataGridView1.DataSource).List;

            CarregaProdutoApi();

            var indexPos = group.FindIndex(t => t.produtoId == tipo.produtoId);
            dataGridView1.Rows[1].Selected = false;
            dataGridView1.Rows[indexPos].Selected = true;

            //CarregaProdutoApi();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            tabControl2.SelectedTab = tabDetalhe;
            toolStripButton1.PerformClick();
        }
    }
}
