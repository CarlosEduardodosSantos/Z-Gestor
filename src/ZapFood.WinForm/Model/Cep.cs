using System;

namespace ZapFood.WinForm.Model
{
    public class Cep
    {
        public string objectId { get; set; }
        public string CEP { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string bairro { get; set; }
        public string logradouro { get; set; }
        public object numero { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

    }

    public class LocalAtendimento
    {
        public int atendimentoLocalId { get; set; }
        public string descricao { get; set; }
        public int restauranteId { get; set; }
        public string faixaInicial { get; set; }
        public string faixaFinal { get; set; }
        public decimal valorEntrega { get; set; }
    }

    public class RootCep
    {
        public Cep[] results { get; set; }
    }
}