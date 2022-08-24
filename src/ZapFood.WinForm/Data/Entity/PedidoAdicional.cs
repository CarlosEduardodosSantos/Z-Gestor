using System.Xml.Serialization;

namespace ZapFood.WinForm.Data.Entity
{
    [XmlRoot("adicional")]
    public class PedidoAdicional
    {
        [XmlElement("Codigo")]
        public string Codigo { get; set; }
        [XmlElement("CodPdv")]
        public string CodPdv { get; set; }
        [XmlElement("Descricao")]
        public string Descricao { get; set; }
        [XmlElement("Quantidade")]
        public string Quantidade { get; set; }
        [XmlElement("ValorUnit")]
        public string ValorUnit { get; set; }
    }
}