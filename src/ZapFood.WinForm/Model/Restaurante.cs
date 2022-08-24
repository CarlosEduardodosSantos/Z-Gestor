using System;

namespace ZapFood.WinForm.Model
{
    public class Restaurante
    {
        public int RestauranteId { get; set; }
        public Guid Token { get; set; }
        public int Situacao { get; set; }
        public string Status { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Ie { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Fone { get; set; }
        public string Imagem { get; set; }
        public string ImagemBase64 { get; set; }
        public DateTime AbreAs { get; set; }
        public DateTime FechaAs { get; set; }
        public double AvaliacaoRating { get; set; }
        public string Grupo { get; set; }
        public decimal ValorEntrega { get; set; }
        public bool AtendeLocal { get; set; }

        public string TempoEntrega { get; set; }
        public Guid SetorId { get; set; }
        public string Descricao { get; set; }
        public bool AceitaRetira { get; set; }
        public decimal PedidoMinimo { get; set; }
        public string Zimmer { get; set; }
    }
}