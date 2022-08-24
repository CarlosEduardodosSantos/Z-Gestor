using System;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Data.Entity
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public int ReferenciaId { get; set; }
        public int RestauranteId { get; set; }
        public string RestauranteToken { get; set; }
        public string Descricao { get; set; }
        public SituacaoCadastro Situacao { get; set; }
        public string SituacaoStr => Situacao.ToString();
        public int Sequencia { get; set; }
    }

    public class Complemento
    {
        public int ComplementoId { get; set; }
        public string TokenRestaurante { get; set; }
        public int ReferenciaId { get; set; }
        public int RestauranteId { get; set; }
        public string Descricao { get; set; }
        public int CategoriaId { get; set; }
        public decimal Valor { get; set; }
    }
}