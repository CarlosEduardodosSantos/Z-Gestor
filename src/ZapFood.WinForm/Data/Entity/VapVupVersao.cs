using System;

namespace ZapFood.WinForm.Data.Entity
{
    public class VapVupVersao
    {
        public VapVupVersao()
        {
            VersaoId = Guid.NewGuid();
            DataInstalacao = DateTime.Now;
        }
        public Guid VersaoId { get; set; }
        public DateTime DataInstalacao { get; set; }
        public string VersaoAtual { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}