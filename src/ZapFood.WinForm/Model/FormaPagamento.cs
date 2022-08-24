using System;
using System.Collections.Generic;

namespace ZapFood.WinForm.Model
{
    public class FormaPagamento
    {
        public FormaPagamento()
        {
            FormaPagamentoId = Guid.NewGuid();
            Situacao = 1;
            RestauranteToken = Program.Restaurante.Token;
        }
        public Guid FormaPagamentoId { get; set; }
        public int Situacao { get; set; }
        public Guid RestauranteToken { get; set; }
        public string Descricao { get; set; }
        public bool IsOnline { get; set; }
        public bool IsTroco { get; set; }
        public int TipoCartao { get; set; }
        public int Sequencia { get; set; }
        public decimal Percentual { get; set; }
        public string Imagem { get; set; }
    }

    public class RootFormaPagamento
    {
        public RootFormaPagamento()
        {
            results = new List<FormaPagamento>();
        }
        public int totalPage { get; set; }
        public List<FormaPagamento> results { get; set; }
    }
}