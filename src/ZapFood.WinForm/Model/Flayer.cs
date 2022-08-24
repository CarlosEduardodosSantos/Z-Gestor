using System;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm.Model
{

    public class Flayer
    {
        public Flayer()
        {
            restauranteId = Program.Restaurante.RestauranteId;
            flyerGuid = Guid.NewGuid();
        }
        public Guid flyerGuid { get; set; }
        public string title { get; set; }
        public string details { get; set; }
        public string picture { get; set; }
        public int produtoId { get; set; }
        public Produto produto { get; set; }
        public int restauranteId { get; set; }
    }
}