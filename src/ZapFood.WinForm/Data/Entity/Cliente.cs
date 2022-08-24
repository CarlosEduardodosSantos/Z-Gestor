using System.Xml.Serialization;

namespace ZapFood.WinForm.Data.Entity
{
    [XmlRoot("cliente")]
    public class Cliente
    {
        [XmlElement("codigo")]
        public string Codigo { get; set; }
        [XmlElement("nome")]
        public string Nome { get; set; }
        [XmlElement("telefone")]
        public string Telefone { get; set; }
        [XmlElement("email")]
        public string Email { get; set; }
    }
}