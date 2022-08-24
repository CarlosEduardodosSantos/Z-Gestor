using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZapFood.WinForm.Data.Entity
{
    [XmlRoot("pedido")]
    public class Pedido
    {
        [XmlElement("nroPedido")]
        public string NroPedido { get; set; }
        [XmlElement("retira")]
        public bool Retira { get; set; }
        [XmlElement("ValorTotal")]
        public decimal ValorTotal { get; set; }
        [XmlElement("ValorTaxa")]
        public decimal ValorTaxa { get; set; }
        [XmlElement("TrocoPara")]
        public string TrocoPara { get; set; }
        [XmlElement("CodPdvPagamento")]
        public string CodPdvPagamento { get; set; }
        [XmlElement("DescricaoPagamento")]
        public string DescricaoPagamento { get; set; }
        [XmlElement("ObsGeraisPedido")]
        public string ObsGeraisPedido { get; set; }
        [XmlElement("CodigoFilial")]
        public string CodigoFilial { get; set; }
        [XmlElement("StatusPedido")]
        public string StatusPedido { get; set; }
        [XmlElement("DataHoraPedido")]
        public string DataHoraPedido { get; set; }
        [XmlElement("PedidoCPF")]
        public string PedidoCPF { get; set; }
        public bool Pendente { get; set; }
        [XmlElement("cliente")]
        public Cliente Cliente { get; set; }
        [XmlElement("endereco")]
        public Endereco Endereco { get; set; }
        [XmlElement("itens")]
        public Itens Itens { get; set; }

    }

    public class Itens
    {
        [XmlElement("item")]
        public List<PedidoItem> PedidoItens { get; set; }
    }
}