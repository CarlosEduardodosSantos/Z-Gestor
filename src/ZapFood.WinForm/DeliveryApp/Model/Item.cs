﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZapFood.WinForm.Model.DeliveryApp
{
    public class item
    {
        public item()
        {
            ComplementCategories = new List<complement_categorie>();
        }

        public string title { get; set; }
        public int quantity { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public decimal total { get; set; }
        public string notes { get; set; }

        [JsonProperty(PropertyName = "ref")]
        public string _ref { get; set; }
        public string variacao_ref { get; set; }
        public string category { get; set; }

        public List<complement_categorie> ComplementCategories { get; set; }
    }
}
