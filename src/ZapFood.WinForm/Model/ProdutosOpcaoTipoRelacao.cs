using System;

namespace ZapFood.WinForm.Model
{
    public class ProdutosOpcaoTipoRelacao
    {
        public ProdutosOpcaoTipoRelacao()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid ProdutosOpcaoId { get; set; }
        public int ProdutoId { get; set; }
    }
}