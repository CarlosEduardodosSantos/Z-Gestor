using System.Collections.Generic;
using ZapFood.WinForm.Data.Entity;

namespace ZapFood.WinForm.Model
{
    public class ProdutoRoot
    {
        public ProdutoRoot()
        {
            results = new List<Produto>();
        }
        public int totalPage { get; set; }
        public List<Produto> results { get; set; }
    }
}