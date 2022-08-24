using System;
using System.Collections.Generic;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm.Model
{
    public class User
    {
        public Guid userID { get; set; }
        public string accessKey { get; set; }
        public string email { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string fone { get; set; }
        public string senha { get; set; }
        public string playersId { get; set; }
        public bool isValid { get; set; }
    }

    public class PedidoEntrega
    {
        public Guid pedidoEntregaGuid { get; set; }
        public Guid pedidoGuid { get; set; }
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string unidade { get; set; }
        public string numero { get; set; }
    }

    public class PedidoItem
    {
        public Guid pedidoItemGuid { get; set; }
        public Guid pedidoGuid { get; set; }
        public int produtoId { get; set; }
        public Produto produto { get; set; }
        public string produtoNome => ObterDescricao();
        public string observacao { get; set; }
        public decimal valorprodutos { get; set; }
        public decimal valortotal { get; set; }
        public decimal quantidade { get; set; }
        public List<PedidoComplementos> pedidoComplementos { get; set; }

        private string ObterDescricao()
        {
            string descricao = produto.nome;
            foreach (var pedidoComplementose in pedidoComplementos)
            {
                descricao += " +"+pedidoComplementose.descricao;
            }

            return descricao;
        }
    }

    public class PedidoViewModel
    {
        public Guid pedidoGuid { get; set; }
        public int pedidoId { get; set; }
        public Guid usuarioGuid { get; set; }
        public User user { get; set; }
        public DateTime dataHora { get; set; }
        public int restauranteId { get; set; }
        public int situacao { get; set; }
        public bool isEntrega { get; set; }
        public int indPag { get; set; }
        public string formaPagamento { get; set; }
        public decimal troco { get; set; }
        public decimal subtotal { get; set; }
        public decimal descontoTotal { get; set; }
        public decimal entregaTotal { get; set; }
        public decimal total { get; set; }
        public string coupom { get; set; }
        public string cpf { get; set; }
        public string observacao { get; set; }
        public PedidoEntrega pedidoEntrega { get; set; }
        public List<PedidoItem> pedidoItens { get; set; }
        public bool Pendente { get; set; }
    }

    public class RootObject
    {
        public RootObject()
        {
            results = new List<PedidoViewModel>();
        }
        public int totalPage { get; set; }
        public List<PedidoViewModel> results { get; set; }
    }

    public class PedidoComplementos
    {
        public int pedidoComplementoId { get; set; }
        public Guid pedidoItemGuid { get; set; }
        public Guid pedidoGuid { get; set; }
        public string descricao { get; set; }
        public decimal valor { get; set; }

    }
}