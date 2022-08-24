using System;

namespace ZapFood.WinForm.Model
{
    public class Estado
    {
        public Guid EstadoId { get; set; }
        public string estado { get; set; }
        public string FaixaInicial  { get; set; }
        public string FaixaFinal { get; set; }
    }
}