using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{   
    public class FlayerService
    {
        public List<Flayer> ObterTodas()
        {
            var flyers = new List<Flayer>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/flyer/byrestid/{Program.Restaurante.RestauranteId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        flyers = JsonConvert.DeserializeObject<List<Flayer>>(xml);

                        return flyers;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return flyers;
            }
        }

        public string Adicionar(Flayer flayer)
        {
            var flayerJson = JsonConvert.SerializeObject(flayer);
            var httpContent = new StringContent(flayerJson, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/flyer/", httpContent);

                if (response.Result.IsSuccessStatusCode)
                {
                    string xml = response.Result.Content.ReadAsStringAsync().Result;
                    var result = xml;
                    return result;
                }
                else
                {
                    return response?.Exception?.Message;
                }
            }
        }
        public string Alterar(Flayer flayer)
        {
            var flayerJson = JsonConvert.SerializeObject(flayer);
            var httpContent = new StringContent(flayerJson, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/flyer/", httpContent);

                if (response.Result.IsSuccessStatusCode)
                {
                    string xml = response.Result.Content.ReadAsStringAsync().Result;
                    var result = xml;
                    return result;
                }
                else
                {
                    return response?.Exception?.Message;
                }
            }
        }

        public string Excluir(Flayer flayer)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/flyer/{flayer.flyerGuid}");

                if (response.Result.IsSuccessStatusCode)
                {
                    string xml = response.Result.Content.ReadAsStringAsync().Result;
                    var result = xml;
                    return result;
                }
                else
                {
                    return response?.Exception?.Message;
                }
            }
        }
    }
}
