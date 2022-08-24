using System.Xml.Serialization;

namespace ZapFood.WinForm.Data.Entity
{
    [XmlRoot("parte")]
    public class PedidoParte
    {
        [XmlElement("CodPdvItem")]
        public string CodPdvItem { get; set; }
        [XmlElement("ObsPart")]
        public string ObsPart { get; set; }
        [XmlElement("CodPdvGrupo")]
        public string CodPdvGrupo { get; set; }
    }
}