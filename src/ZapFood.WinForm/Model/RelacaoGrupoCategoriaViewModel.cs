using System;

namespace ZapFood.WinForm.Model
{
    public class RelacaoGrupoCategoriaViewModel
    {
        public RelacaoGrupoCategoriaViewModel()
        {
            GrupoCategoriaId = Guid.NewGuid();
        }
        public Guid GrupoCategoriaId { get; set; }
        public int RestauranteId { get; set; }
        public Guid GupoId { get; set; }
        public Guid GrupoId { get; set; }
        public string NomeGrupo { get; set; }
        public int CategoriaId { get; set; }
        public string NomeCategoria { get; set; }
        public string Imagem { get; set; }  
    }
}