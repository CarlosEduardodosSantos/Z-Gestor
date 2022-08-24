using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Service;

namespace ZapFood.WinForm
{
    public partial class FormRelacaoGrupoCategoria : Form
    {
        private readonly ProdutoGrupoService _produtoGrupoService;
        private readonly ProdutoGrupo _produtoGrupo;

        public FormRelacaoGrupoCategoria(ProdutoGrupo produtoGrupo)
        {
            _produtoGrupo = produtoGrupo;
            InitializeComponent();
            _produtoGrupoService = new ProdutoGrupoService();
        }

        private void FormRelacaoGrupoCategoria_Load(object sender, EventArgs e)
        {
            label1.Text = $"Adicionar categoria ao grupo: {_produtoGrupo.Nome}";
            CarregaCategoria();
            CarregaGrid();
        }

        private void CarregaGrid()
        {
            var categorias = _produtoGrupo.Categorias;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = categorias;
        }

        private void CarregaCategoria()
        {
            var categoriaService = new CategoriaService();
            var rootCategorias = categoriaService.ObterCategorias(99, 1).ResultToList<Categoria>();

            cbCategoria.DataSource = rootCategorias.Where(t => t.Situacao == SituacaoCadastro.Ativo).ToList();
            cbCategoria.DisplayMember = "Descricao";
            cbCategoria.ValueMember = "CategoriaId";
            cbCategoria.SelectedIndex = -1;
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (cbCategoria.SelectedIndex == -1)
            {
                Funcoes.Mensagem("Selecione uma categoria", "Validação", MessageBoxButtons.OK);
                return;
            }

            var categoria = (Categoria)cbCategoria.SelectedItem;

            if (_produtoGrupo.Categorias.Any(t => t.CategoriaId == categoria.CategoriaId))
            {
                Funcoes.Mensagem($"Categoria ja relacionada com o Grupo selecionado.", "Erro", MessageBoxButtons.OK);
                return;
            }


            var relacao = new RelacaoGrupoCategoriaViewModel()
            {
                CategoriaId = categoria.CategoriaId,
                GrupoId = _produtoGrupo.GupoId
            };

            try
            {
                _produtoGrupoService.AdicionarRelacao(relacao);
                _produtoGrupo.Categorias.Add(categoria);
                CarregaGrid();

            }
            catch (Exception exception)
            {
                Funcoes.Mensagem($"Ocorreu um erro ao gravar a relação.\n{exception.Message}", "Erro", MessageBoxButtons.OK);
            }
            

        }
    }
}
