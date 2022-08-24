using System;
using LiteDB;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Data.Entity
{
    public class PedidoVapVupt
    {
        [BsonId]
        public Guid VapVuptId { get; set; }
        public string PedidoId { get; set; }
        public string VendaId { get; set; }
        public DateTime DataHora { get; set; }
        public string FileJson { get; set; }
        public string TipoPedido { get; set; }
        public SituacaoVapVuptEnum Situacao { get; set; }
        public AplicacaoEnum Aplicacao { get; set; }
        public virtual PedidoRootModel PedidoRootModel => GetToJson();

        private PedidoRootModel GetToJson()
        {
            if (FileJson != null)
                return JsonConvert.DeserializeObject<PedidoRootModel>(FileJson);

            return null;
        }
    }
}