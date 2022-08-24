using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZapFood.WinForm.Data.Entity
{

    [XmlRoot("item")]
    public class PedidoItem
    {
        [XmlElement("Codigo")]
        public string Codigo { get; set; }
        [XmlElement("CodPdv")]
        public string CodPdv { get; set; }
        [XmlElement("CodPdvGrupo")]
        public string CodPdvGrupo { get; set; }
        [XmlElement("Descricao")]
        public string Descricao { get; set; }
        [XmlElement("Quantidade")]
        public decimal Quantidade { get; set; }
        [XmlElement("ValorUnit")]
        public decimal ValorUnit { get; set; }
        public decimal ValorTotal => (Quantidade*ValorUnit);

        [XmlElement("ObsItem")]
        public string ObsItem { get; set; }
        [XmlElement("parte")]
        public PedidoParte Parte { get; set; }
        [XmlElement("adicionais")]
        public Adicionais Adicionais { get; set; }
    }

    public class Adicionais
    {
        [XmlElement("adicional")]
        public List<PedidoAdicional> Adicional { get; set; }

    }
}