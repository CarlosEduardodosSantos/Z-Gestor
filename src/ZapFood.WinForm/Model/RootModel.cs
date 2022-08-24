using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ZapFood.WinForm.Model
{
    public class RootModel
    {
        public RootModel()
        {
            results = new List<complementosMeioMeio>();
        }
        public int totalPage { get; set; }
        public List<complementosMeioMeio> results { get; set; }
    }

    public class ComplementosDg
    {
        public string Complementos { get; set; }
    }
}
