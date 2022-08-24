using System.Xml.Serialization;

namespace ZapFood.WinForm.Data.Entity
{
    [XmlRoot("endereco")]
    public class Endereco
    {
        [XmlElement("rua")]
        public string Rua { get; set; }
        [XmlElement("numero")]
        public string Numero { get; set; }
        [XmlElement("complemento")]
        public string Complemento { get; set; }
        [XmlElement("referencia")]
        public string Referencia { get; set; }
        [XmlElement("bairro")]
        public string Bairro { get; set; }
        [XmlElement("cidade")]
        public string Cidade { get; set; }
        [XmlElement("estado")]
        public string Estado { get; set; }
    }
}