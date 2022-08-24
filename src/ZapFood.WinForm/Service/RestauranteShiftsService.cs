using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class RestauranteShiftsService
    {
        public List<RestauranteShifts> ObterFormaPagamentos()
        {

            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/restauranteShifts/obterByToken/{Program.TokenVapVupt}/999/1");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var rootFormaPagamentos = JsonConvert.DeserializeObject<RootRestauranteShifts>(xml);

                        return rootFormaPagamentos.results;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return new List<RestauranteShifts>();
            }
        }
        
        public ResultService Incluir(RestauranteShifts restauranteShifts)
        {

            var restauranteShiftsJson = JsonConvert.SerializeObject(restauranteShifts);
            var httpContent = new StringContent(restauranteShiftsJson, Encoding.UTF8, "application/json");



            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/restauranteShifts/adicionar", httpContent);

                if (response.Result.IsSuccessStatusCode)
                {
                    var xml = response.Result.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<ResultService>(xml);
                    return result;
                }
                else
                {
                    return new ResultService()
                    {
                        Errors = true,
                        Message = response?.Exception?.Message
                    };
                }
            }
        }

        public ResultService Excluir(RestauranteShifts restauranteShifts)
        {

            var id = restauranteShifts.RestauranteShiftsId.ToString();

            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/restauranteShifts/excluir/{id}");

                if (response.Result.IsSuccessStatusCode)
                {
                    var xml = response.Result.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<ResultService>(xml);
                    return result;
                }
                else
                {
                    return new ResultService()
                    {
                        Errors = true,
                        Message = response?.Result?.StatusCode.ToString()
                    };
                }
            }
        }
    }
}