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
    public partial class FrmFlayerCadastro : Form
    {
        private readonly FlayerService _flayerService;
        private List<Flayer> _flayers;
        private Flayer _flayer;
        private bool _isAlter;
        public FrmFlayerCadastro()
        {
            InitializeComponent();
            _flayerService = new FlayerService();
        }

        private void FrmFlayerCadastro_Load(object sender, EventArgs e)
        {
            txtDescricao.ReadOnly = true;
            txtNome.ReadOnly = true;
            btnAdicionar.Enabled = false;
            //btnRelacionar.Enabled = false;
            btnAddImage.Enabled = true;

            btnNovo.Visible = true;


            CarregaOpcoes();
        }

        void CarregaOpcoes()
        {
            _flayers = _flayerService.ObterTodas().ToList();


            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.DataSource = _flayers;
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
                Funcoes.Mensagem("É obrigatório informar o nome para sua campanha", "Validação", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(txtDescricao.Text))
            {
                Funcoes.Mensagem("É obrigatório informar o descrição para sua campanha", "Validação", MessageBoxButtons.OK);
                return;
            }
            if (!_isAlter)
            {
                var flayer = new Flayer()
                {
                    title = txtNome.Text,
                    details = txtDescricao.Text,
                    picture = GetImagem(),
                };

                _flayerService.Adicionar(flayer);
                Funcoes.Mensagem("Campanha foi criada com sucesso.", "Informação", MessageBoxButtons.OK);
            }
            else
            {
                _flayer.title = txtNome.Text;
                _flayer.details = txtDescricao.Text;
                _flayer.picture = GetImagem();
                _flayerService.Alterar(_flayer);
                Funcoes.Mensagem("Campanha foi alterada com sucesso.", "Informação", MessageBoxButtons.OK);
            }



            CarregaOpcoes();

            txtDescricao.Clear();
            txtNome.Clear();
            txtLabelGrupo.Text = "";
            pictureBox1.Image = null;

            txtDescricao.ReadOnly = true;
            txtNome.ReadOnly = true;
            btnAdicionar.Enabled = false;
            //btnRelacionar.Enabled = false;
            btnAddImage.Enabled = false;

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

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma companha para alteração");
                return;
            }

            var index = dataGridView3.SelectedRows[0].Index;

            Flayer relacao;
            if (dataGridView3.Rows[index].DataBoundItem.GetType() == typeof(Subro.Controls.GroupRow))
                relacao = (Flayer)((Subro.Controls.GroupRow) dataGridView3.Rows[index].DataBoundItem).FirstRow;
            else
                relacao = (Flayer)dataGridView3.Rows[index].DataBoundItem;

            var tipo = _flayers.FirstOrDefault(t => t.flyerGuid == relacao.flyerGuid);

            if (index == 0)
            {
                _flayerService.Alterar(tipo);
                return;
            }


            var indexAlter = index - 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter + 2;

            var relacao2 = (Flayer)dataGridView3.Rows[indexAlter].DataBoundItem;
            var tipo2 = _flayers.FirstOrDefault(t => t.flyerGuid == relacao2.flyerGuid);

            _flayerService.Alterar(tipo);


            _flayerService.Alterar(tipo2);

            CarregaOpcoes();

            var indexPos = _flayers.FindIndex(t => t.flyerGuid == tipo.flyerGuid);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma companha para alteração");
                return;
            }

            var index = dataGridView3.SelectedRows[0].Index;

            Flayer relacao;
            if (dataGridView3.Rows[index].DataBoundItem.GetType() == typeof(Subro.Controls.GroupRow))
                relacao = (Flayer)((Subro.Controls.GroupRow)dataGridView3.Rows[index].DataBoundItem).FirstRow;
            else
                relacao = (Flayer)dataGridView3.Rows[index].DataBoundItem;

            var tipo = _flayers.FirstOrDefault(t => t.flyerGuid== relacao.flyerGuid);

            if (index == dataGridView3.RowCount - 1)
            {
                return;
            }


            var indexAlter = index + 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter;

            var relacao2 = (Flayer)dataGridView3.Rows[indexAlter].DataBoundItem;
            var tipo2 = _flayers.FirstOrDefault(t => t.flyerGuid== relacao2.flyerGuid);

            _flayerService.Alterar(tipo);


            _flayerService.Alterar(tipo2);

            CarregaOpcoes();

            var indexPos = _flayers.FindIndex(t => t.flyerGuid== tipo.flyerGuid);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var index = dataGridView3.SelectedRows[0].Index;
            var relacao = (Flayer)dataGridView3.Rows[index].DataBoundItem;
            _flayer = _flayers.FirstOrDefault(t => t.flyerGuid == relacao.flyerGuid);

            if(_flayer == null)return;

            txtDescricao.Text = _flayer.details;
            txtNome.Text = _flayer.title;
            txtLabelGrupo.Text = txtNome.Text;

            pictureBox1.Image = null;

            if (!string.IsNullOrEmpty(_flayer.picture))
            {
                try
                {
                    pictureBox1.Load($"{_flayer.picture}");
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
           // btnRelacionar.Enabled = true;
            btnExcluir.Enabled = true;
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {

            txtDescricao.Clear();
            txtNome.Clear();
            txtDescricao.ReadOnly = false;
            txtNome.ReadOnly = false;
            btnAddImage.Visible = true;
            //btnRelacionar.Enabled = false;
            btnExcluir.Enabled = false;
            _isAlter = false;

            _flayer = new Flayer();
            btnAdicionar.Enabled = true;
            txtNome.Focus();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if(_flayer == null)return;
            var result = MessageBox.Show($"Deseja realmente excluir o Horario selecionado?",
            "Cadastro de Produto", MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result != DialogResult.Yes && _flayer != null)
            {
                return;
            }
            else
            {
                _flayerService.Excluir(_flayer);
                Funcoes.Mensagem("Horário de funcionamento foi excluido com sucesso.", "Informação", MessageBoxButtons.OK);
                CarregaOpcoes();
            }
            MessageBox.Show("Campanha excluida com sucesso");

            CarregaOpcoes();

            txtDescricao.Clear();
            txtNome.Clear();
            txtLabelGrupo.Text = "";
            pictureBox1.Image = null;
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

        private void txtNome_Leave(object sender, EventArgs e)
        {
            txtLabelGrupo.Text = txtNome.Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
             //   if (e.ColumnIndex != 9) return;

             //   var abertura = (ProdutoShifts)dataGridView3.Rows[e.RowIndex].DataBoundItem;
             //   _produtoShiftService.Excluir(abertura);
             //   Funcoes.Mensagem("Horário de funcionamento foi excluido com sucesso.", "Informação", MessageBoxButtons.OK);
             //   CarregaOpcoes();
            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
