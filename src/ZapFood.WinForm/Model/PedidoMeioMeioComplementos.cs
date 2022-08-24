using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapFood.WinForm.Model
{
    public class complementosMeioMeio
    {
        public int PedidoMeioMeioComplementoID { get; set; }
        public string Complemento { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public Guid MeioMeioGuid { get; set; }

    }
}
