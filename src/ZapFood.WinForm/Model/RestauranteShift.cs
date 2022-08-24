using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapFood.WinForm.Model
{
    public class RestauranteShifts
    {
        public RestauranteShifts()
        {
            RestauranteId = Program.Restaurante.RestauranteId;
            RestauranteShiftsId = Guid.NewGuid();
        }
        public Guid RestauranteShiftsId { get; set; }
        public int RestauranteId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool monday { get; set; }
        public bool tuesday { get; set; }
        public bool wednesday { get; set; }
        public bool thursday { get; set; }
        public bool friday { get; set; }
        public bool saturday { get; set; }
        public bool sunday { get; set; }
    }
}
