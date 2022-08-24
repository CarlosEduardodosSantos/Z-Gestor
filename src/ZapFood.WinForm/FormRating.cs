using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormRating : Form
    {
        private readonly RatingService _ratingService;
        private List<Rating> _ratings;
        private Rating _rating;
        public FormRating()
        {
            InitializeComponent();
            _ratingService = new RatingService();
        }

        private void FormRating_Load(object sender, EventArgs e)
        {
            txtNome.ReadOnly = true;
            txtTelefone.ReadOnly = true;
            txtData.ReadOnly = true;
            txtDescricao.ReadOnly = true;


            CarregaOpcoes();
        }

        void CarregaOpcoes()
        {
            _ratings = _ratingService.ObterTodas().ToList();


            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.DataSource = _ratings;
        }

        private void txtLabelGrupo_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {

        }

        private void pnlImagem_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            if (_rating == null) return;

            var result = _ratingService.Excluir(_rating);
            MessageBox.Show("Avaliação excluida com sucesso");

            CarregaOpcoes();

            txtNome.Clear();
            txtTelefone.Clear();
            txtData.Clear();
            txtDescricao.Clear();
            label7.Text = "";
            label8.Text = "";

        }

        private void dataGridView3_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        { 
        }

        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            {
                var index = dataGridView3.SelectedRows[0].Index;
                var relacao = (Rating)dataGridView3.Rows[index].DataBoundItem;
                _rating = _ratings.FirstOrDefault(t => t.RestauranteRatingGuid == relacao.RestauranteRatingGuid);

                if (_rating == null) return;

                txtNome.Text = _rating.Name;
                txtTelefone.Text = _rating.Phone;
                txtData.Text = _rating.DataHora.ToString();
                txtDescricao.Text = _rating.Suggestion;
                label8.Text = _rating.Value.ToString();
                label7.Text = "*";

                txtDescricao.ReadOnly = true;
                txtNome.ReadOnly = true;
                txtData.ReadOnly = true;
                txtTelefone.ReadOnly = true;
                btnExcluir.Enabled = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtNota_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
