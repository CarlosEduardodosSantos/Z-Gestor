using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using ZapFood.WinForm.Model;


namespace ZapFood.WinForm.Service
{
    class RestauranteShiftService
    {
        public List<RestauranteShifts> ObterTodas()
        {
            var restauranteShifts = new List<RestauranteShifts>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/RestauranteShifts/ByRestId/{Program.Restaurante.RestauranteId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        restauranteShifts = JsonConvert.DeserializeObject<List<RestauranteShifts>>(xml);

                        return restauranteShifts;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return restauranteShifts;
            }
        }

        public string Adicionar(RestauranteShifts restauranteShifts)
        {
            var restShiftsJson = JsonConvert.SerializeObject(restauranteShifts);
            var httpContent = new StringContent(restShiftsJson, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/RestauranteShifts/", httpContent);

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

        public string Excluir(RestauranteShifts restauranteShifts)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/RestauranteShifts/{restauranteShifts.RestauranteShiftsId}");

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
