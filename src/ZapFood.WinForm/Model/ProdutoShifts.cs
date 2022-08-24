using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapFood.WinForm.Model
{
    class ProdutoShifts
    {
        public ProdutoShifts()
        {
            ProdutoId = Program.Restaurante.RestauranteId;
            ProdutoShiftsGuid = Guid.NewGuid();
        }
        public Guid ProdutoShiftsGuid { get; set; }
        public int ProdutoId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
    }
}