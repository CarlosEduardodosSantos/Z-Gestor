using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;


namespace ZapFood.WinForm
{
    public partial class FormProfile : Form
    {
        private readonly LocalizacaoService _localizacaoService;
        private readonly RestauranteService _restauranteService;
        private readonly FormaPagamentoService _formaPagamentoService;
        private readonly RestauranteShiftService _restauranteShiftService;

        private List<LocalAtendimento> _localAtendimentos;
        private List<FormaPagamento> _formaPagamentos;
        private List<RestauranteShifts> _restauranteShiftses;

        public FormProfile()
        {
            InitializeComponent();
            _localizacaoService = new LocalizacaoService();
            _restauranteService = new RestauranteService();
            _formaPagamentoService = new FormaPagamentoService();
            _restauranteShiftService = new RestauranteShiftService();
            _localAtendimentos = new List<LocalAtendimento>();
            _formaPagamentos = new List<FormaPagamento>();
        }


        private void FormProfile_Load(object sender, EventArgs e)
        {
            txtEmpresa.Text = Program.Restaurante.Nome;
            txtCnpj.Text = Program.Restaurante.Cnpj;
            cbAtividade.Text = Program.Restaurante.Grupo;
            dthHoraInicio.Value = Program.Restaurante.AbreAs;
            dthHoraFinal.Value = Program.Restaurante.FechaAs;
            txtTempoEntrega.Text = Program.Restaurante.TempoEntrega;
            chkRetiraLocal.Checked = Program.Restaurante.AceitaRetira;
            txtValorMinimo.ValueNumeric = Program.Restaurante.PedidoMinimo;
            txtZimmer.Text = Program.Restaurante.Zimmer;

            CarregaOpcoes();
        }

            void CarregaOpcoes()
            {
                _restauranteShiftses = _restauranteShiftService.ObterTodas().ToList();


                dataGridView4.AutoGenerateColumns = false;
                dataGridView4.DataSource = _restauranteShiftses;


            if (string.IsNullOrEmpty(Program.Restaurante.Imagem)) return;

            try
            {
                byte[] imageBytes = Convert.FromBase64String(Program.Restaurante.Imagem.Replace("data:image/png;base64,", "").Replace("data:image/jpeg;base64,", ""));
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            catch
            {
                //Ignore
            }

       }

        private void CbEstadoOnSelectionChangeCommitted(object sender, EventArgs eventArgs)
        {
            if (cbEstado.SelectedItem == null) return;

            var cidades = _localizacaoService.ObterCidades(cbEstado.Text);

            cbCidade.DataSource = cidades;
            cbCidade.DisplayMember = "cidade";
            cbCidade.SelectedIndex = -1;
            cbCidade.SelectionChangeCommitted += CbCidadeOnSelectionChangeCommitted;

        }

        private void CbCidadeOnSelectionChangeCommitted(object sender, EventArgs eventArgs)
        {
            if (cbCidade.SelectedItem == null) return;

            var bairros = _localizacaoService.ObterBairros(cbEstado.Text, cbCidade.Text);
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = bairros;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0) return;

            var item = (Bairro)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            var valorEntrega = FormInformeValor.ObterValor();
            var local = new LocalAtendimento()
            {
                descricao = item.bairro,
                faixaInicial = item.FaixaInicial,
                faixaFinal = item.FaixaFinal,
                restauranteId = Program.Restaurante.RestauranteId,
                valorEntrega = valorEntrega
                
            };

            IncluirLocalAtendimento(local);
        }

        private void CarregaBairrosAtendidos()
        {
            _localAtendimentos = _restauranteService.ObterLocaisDeAtendimento(Program.Restaurante.RestauranteId);
            dataGridView2.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = _localAtendimentos;
        }

        void CarregaFormaPagamentos()
        {
            _formaPagamentos = _formaPagamentoService.ObterFormaPagamentos();
            dataGridView3.DataSource = null;
            dataGridView3.AutoGenerateColumns = false;
            dataGridView3.DataSource = _formaPagamentos;
        }

        private void btnEnviarTodos_Click(object sender, EventArgs e)
        {
            if (cbCidade.SelectedIndex == -1) return;

            var cidade = (Cidade)cbCidade.SelectedItem;

            var valorEntrega = FormInformeValor.ObterValor();
            var local = new LocalAtendimento()
            {
                descricao = cidade.cidade,
                faixaInicial = cidade.FaixaInicial,
                faixaFinal = cidade.FaixaFinal,
                restauranteId = Program.Restaurante.RestauranteId,
                valorEntrega = valorEntrega
            };

            IncluirLocalAtendimento(local);

        }

        private void IncluirLocalAtendimento(LocalAtendimento localAtendimento)
        {
            var result = _localAtendimentos.Any(t => t.descricao == localAtendimento.descricao)
                ? _restauranteService.AlterarLocalAtendimento(localAtendimento)
                : _restauranteService.IncluirLocalAtendimento(localAtendimento);

            if (result.Errors)
            {
                MessageBox.Show(result.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CarregaBairrosAtendidos();
        }

        void IncluirFormaPagamento(FormaPagamento formaPagamento)
        {
            var result = _formaPagamentos.Any(t => t.Descricao == formaPagamento.Descricao)
                ? _formaPagamentoService.Alterar(formaPagamento)
                : _formaPagamentoService.Incluir(formaPagamento);

            if (result.Errors)
            {
                MessageBox.Show(result.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CarregaFormaPagamentos();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var arquivo = Funcoes.BuscarImagem();
            if (string.IsNullOrEmpty(arquivo.FileName)) return;
            var imagem = Image.FromFile(arquivo.FileName);
            pictureBox1.Image = imagem;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {

                CarregaBairrosAtendidos();

                var estados = _localizacaoService.ObterEstados();
                cbEstado.DataSource = estados;
                cbEstado.DisplayMember = "estado";
                cbEstado.SelectedIndex = -1;
                cbEstado.SelectionChangeCommitted += CbEstadoOnSelectionChangeCommitted;
            }
            else if (tabControl1.SelectedTab == tabPagamentos)
            {
                CarregaFormaPagamentos();

                var especies = _formaPagamentoService.ObterEspecies();
                dgvEspecies.AutoGenerateColumns = false;
                dgvEspecies.DataSource = especies;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.Restaurante.Imagem = $"data:image/jpeg;base64,{GetImagem()}";
            Program.Restaurante.TempoEntrega = txtTempoEntrega.Text;
            Program.Restaurante.AbreAs = dthHoraInicio.Value;
            Program.Restaurante.FechaAs = dthHoraFinal.Value;
            Program.Restaurante.AceitaRetira = chkRetiraLocal.Checked;
            Program.Restaurante.PedidoMinimo = txtValorMinimo.ValueNumeric;
            Program.Restaurante.Zimmer = txtZimmer.Text;

            var result = _restauranteService.AlterarRestaurante(Program.Restaurante);
            MessageBox.Show(result.Message, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2) return;

            var local = (LocalAtendimento)dataGridView2.Rows[e.RowIndex].DataBoundItem;
            _restauranteService.ExcluirLocalAtendimento(local);
            CarregaBairrosAtendidos();
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var local = (LocalAtendimento)dataGridView2.Rows[e.RowIndex].DataBoundItem;
            IncluirLocalAtendimento(local);
        }

        private void dgvEspecies_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvEspecies.SelectedCells.Count == 0) return;

            var item = (EspeciePagamento)dgvEspecies.Rows[e.RowIndex].DataBoundItem;
            var sequencia = _formaPagamentos.Count + 1;
            var formaPagamento = new FormaPagamento()
            {
                Descricao = item.Nome,
                IsOnline = false,
                IsTroco = item.Nome.Equals("Dinheiro"),
                TipoCartao = 0,
                Sequencia = sequencia,
                Percentual = 0,
                Imagem = item.Icon
            };
            IncluirFormaPagamento(formaPagamento);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvEspecies.Rows)
            {
                var item = (EspeciePagamento)row.DataBoundItem;
                var sequencia = _formaPagamentos.Count + 1;

                var formaPagamento = new FormaPagamento()
                {
                    Descricao = item.Nome,
                    IsOnline = false,
                    IsTroco = item.Nome.Equals("Dinheiro"),
                    TipoCartao = 0,
                    Sequencia = sequencia,
                    Percentual = 0,
                    Imagem = item.Icon
                };
                IncluirFormaPagamento(formaPagamento);

            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if(dataGridView3.SelectedRows.Count == 0)return;

            var index = dataGridView3.SelectedRows[0].Index;
            var formaPagamento = (FormaPagamento)dataGridView3.Rows[index].DataBoundItem;
            if (index == 0)
            {
                formaPagamento.Sequencia = 1;
                _formaPagamentoService.Alterar(formaPagamento);
                CarregaFormaPagamentos();
                return;
            }
            var indexAlter = index - 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter + 2;

            var formaPagamento2 = (FormaPagamento)dataGridView3.Rows[indexAlter].DataBoundItem;

            formaPagamento.Sequencia = sequenciaNovo;
            _formaPagamentoService.Alterar(formaPagamento);


            formaPagamento2.Sequencia = sequenciaOld;
            _formaPagamentoService.Alterar(formaPagamento2);

            CarregaFormaPagamentos();

            var indexPos = _formaPagamentos.FindIndex(t => t.FormaPagamentoId == formaPagamento.FormaPagamentoId);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            var index = dataGridView3.SelectedRows[0].Index;
            var formaPagamento = (FormaPagamento)dataGridView3.Rows[index].DataBoundItem;
            if (index == dataGridView3.RowCount - 1)
            {
                return;
            }
            
            var indexAlter = index + 1;
            var sequenciaNovo = indexAlter + 1;
            var sequenciaOld = indexAlter;

            var formaPagamento2 = (FormaPagamento)dataGridView3.Rows[indexAlter].DataBoundItem;

            formaPagamento.Sequencia = sequenciaNovo;
            _formaPagamentoService.Alterar(formaPagamento);


            formaPagamento2.Sequencia = sequenciaOld;
            _formaPagamentoService.Alterar(formaPagamento2);

            CarregaFormaPagamentos();

            var indexPos = _formaPagamentos.FindIndex(t => t.FormaPagamentoId == formaPagamento.FormaPagamentoId);
            dataGridView3.Rows[indexPos].Selected = true;
        }

        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(dataGridView3.SelectedRows.Count <= 0)
                return;

            var formaPgto = (FormaPagamento)dataGridView3.SelectedRows[0].DataBoundItem;
            var msg = MessageBox.Show($"Confirma a exclusão da forma de pagamento {formaPgto.Descricao}?",
                "Forma de pagamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(msg != DialogResult.Yes)
                return;

            try
            {
                var result = _formaPagamentoService.Excluir(formaPgto);

                MessageBox.Show(result.Message, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CarregaFormaPagamentos();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
                if ((chkMonday.Checked || chkTuesday.Checked || chkWednesday.Checked || 
                    chkThursday.Checked || chkFriday.Checked || chkSaturday.Checked || chkSunday.Checked) == false)
            {
                Funcoes.Mensagem("Preencha o dia da abertura do Restaurante", "Informação", MessageBoxButtons.OK);
            }
            else
            {
                var restauranteShift = new RestauranteShifts()
                {
                    StartTime = dtpStartTime.Value,
                    EndTime = dtpEndTime.Value,
                    monday = chkMonday.Checked,
                    tuesday = chkTuesday.Checked,
                    wednesday = chkWednesday.Checked,
                    thursday = chkThursday.Checked,
                    friday = chkFriday.Checked,
                    saturday = chkSaturday.Checked,
                    sunday = chkSunday.Checked,

                };

                _restauranteShiftService.Adicionar(restauranteShift);
                Funcoes.Mensagem("Horário de funcionamento foi criado com sucesso.", "Informação", MessageBoxButtons.OK);
                CarregaOpcoes();
            }

            }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
                if (e.ColumnIndex != 9) return;

                var abertura = (RestauranteShifts)dataGridView4.Rows[e.RowIndex].DataBoundItem;
                var result = MessageBox.Show($"Deseja realmente excluir o Horario selecionado?",
                "Cadastro de Produto", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (result != DialogResult.Yes && abertura != null)
                {
                    return;
                }
                else
                {
                    _restauranteShiftService.Excluir(abertura);
                    Funcoes.Mensagem("Horário de funcionamento foi excluido com sucesso.", "Informação", MessageBoxButtons.OK);
                    CarregaOpcoes();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void txtZimmer_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}