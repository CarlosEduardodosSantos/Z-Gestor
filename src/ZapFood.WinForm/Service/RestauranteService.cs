using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class RestauranteService
    {
        public Restaurante ObterPorToken(string token)
        {
            var restaurante = new Restaurante();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/restaurante/restauranteByToken/{token}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        restaurante = JsonConvert.DeserializeObject<Restaurante>(xml);

                        return restaurante;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return restaurante;
            }
        }

        public Restaurante ObterPorCnpj(string cnpj)
        {
            var restaurante = new Restaurante();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/restaurante/restauranteByCnpj/{cnpj}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        restaurante = JsonConvert.DeserializeObject<Restaurante>(xml);

                        return restaurante;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return restaurante;
            }
        }
        public bool AlterarStatusLoja(string status)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PUT"), $"{Program.AddressApi}/api/restaurante/putstatuss/{Program.TokenVapVupt}/{status}"))
                {

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }

        public ResultService AlterarRestaurante(Restaurante restaurante)
        {
            var restauranteJson = JsonConvert.SerializeObject(restaurante);
            var httpContent = new StringContent(restauranteJson, Encoding.UTF8, "application/json");



            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/restaurante/alterarRestaurante", httpContent);

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

        #region Localde Atendimento

        public List<LocalAtendimento> ObterLocaisDeAtendimento(int restauranteId)
        {
            var locaisAtendimento = new List<LocalAtendimento>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/restaurante/locaisAtendimentos/{restauranteId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        locaisAtendimento = JsonConvert.DeserializeObject<List<LocalAtendimento>>(xml);

                        return locaisAtendimento;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return locaisAtendimento;
            }
        }

        public ResultService IncluirLocalAtendimento(LocalAtendimento localAtendimento)
        {
            localAtendimento.restauranteId = Program.Restaurante.RestauranteId;
            var localAtendimentoJdon = JsonConvert.SerializeObject(localAtendimento);
            var httpContent = new StringContent(localAtendimentoJdon, Encoding.UTF8, "application/json");



            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/restaurante/adicionarLocalAtendimento", httpContent);

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
        public ResultService AlterarLocalAtendimento(LocalAtendimento localAtendimento)
        {
            localAtendimento.restauranteId = Program.Restaurante.RestauranteId;
            var localAtendimentoJdon = JsonConvert.SerializeObject(localAtendimento);
            var httpContent = new StringContent(localAtendimentoJdon, Encoding.UTF8, "application/json");



            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/restaurante/alterarLocalAtendimento", httpContent);

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

        public ResultService ExcluirLocalAtendimento(LocalAtendimento localAtendimento)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/restaurante/deleteLocalAtendimento/{localAtendimento.atendimentoLocalId}");

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

        #endregion
    }
}