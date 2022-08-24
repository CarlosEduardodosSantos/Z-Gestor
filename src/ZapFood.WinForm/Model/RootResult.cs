

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ZapFood.WinForm.Model
{
    public class RootResult
    {
        public int TotalPage { get; set; }
        public IEnumerable<object> Results { get; set; }

        public List<T> ResultToList<T>() where T : class
        {
            var listString = JsonConvert.SerializeObject(Results);
            return JsonConvert.DeserializeObject<List<T>>(listString);
        }
    }
}