using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.IFoodAutenticate;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;
using RootModel = ZapFood.WinForm.Model.RootModel;

namespace ZapFood.WinForm.Service
{
    public class ComplementosService
    {
        public RootModel ObterComplementosMeioMeio()
        {
            var pedidos = new RootModel();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/ComplementosMeioMeio/ObterTodos");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var pedido = JsonConvert.DeserializeObject<RootModel>(xml);

                        return pedido;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return pedidos;
            }
        }

        public RootModel ObterComplementosMeioMeioPorGuid(string Pedidos)
        {
            var pedidos = new RootModel();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/ComplementosMeioMeio/ObterComplMeioMeioPorGuid/{Pedidos}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var pedido = JsonConvert.DeserializeObject<RootModel>(xml);

                        return pedido;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return pedidos;
            }
        }
    }
}
