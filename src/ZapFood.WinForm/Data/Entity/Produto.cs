
namespace ZapFood.WinForm.Data.Entity
{
    public class Produto
    {
        public bool IsOk { get; set; } = true;
        public int produtoId { get; set; }
        public string TokenRestaurante { get; set; }
        public int situacao { get; set; }
        public int referenciaId { get; set; }
        public int restauranteId { get; set; }
        public string nome { get; set; }
        public decimal valorVenda { get; set; }
        public decimal valorRegular { get; set; }
        public decimal valorPromocao { get; set; }
        public string imagem { get; set; }
        public string imagemBase64 { get; set; }
        public double avaliacaoRating { get; set; }
        public int categoriaId { get; set; }
        public string categoriaNome { get; set; }
        public string descricao { get; set; }
        public int numberMeiomeio { get; set; }
        public bool isPartMeioMeio { get; set; }
        public int tamanhoId { get; set; }
        public string produtoGuid { get; set; }
        public int sequencia { get; set; }
        public bool isControlstock { get; set; }
        public int stock { get; set; }
        public bool isCombo { get; set; }
    }
}