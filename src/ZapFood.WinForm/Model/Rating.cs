using System;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm.Model
{
    public class Rating
    {
        public Rating()
        {
            restauranteId = Program.Restaurante.RestauranteId;
            RestauranteRatingGuid = Guid.NewGuid();
        }

        public int restauranteId { get; set; }
        public DateTime DataHora { get; set; }
        public string Suggestion { get; set; }
        public int Value { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public Guid RestauranteRatingGuid { get; set; }        
    }
}
