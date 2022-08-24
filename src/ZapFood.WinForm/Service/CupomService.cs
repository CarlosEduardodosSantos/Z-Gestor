using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class CupomService
    {
        public RootResult ObterTodos()
        {
            var cupons = new RootResult();
            using (var client = new HttpClient())
            {
                var restauranteId = Program.Restaurante.RestauranteId;
                var response = client.GetAsync($"{Program.AddressApi}/api/cupom/obterByRestauranteId/{restauranteId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        cupons = JsonConvert.DeserializeObject<RootResult>(xml);

                        return cupons;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return cupons;
            }
        }

        public RootResult ObterCupomMovimentacao(string cupomId)
        {
            var cupons = new RootResult();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/cupom/obterCupomMovimentacao/{cupomId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        cupons = JsonConvert.DeserializeObject<RootResult>(xml);

                        return cupons;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return cupons;
            }
        }

        public string Adicionar(Cupom cupom)
        {
            var categoriaJdon = JsonConvert.SerializeObject(cupom);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/cupom/adicionar", httpContent);

                if (response.Result.IsSuccessStatusCode)
                {
                    var xml = response.Result.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<ResultService>(xml);
                    return result.Message;
                }
                else
                {
                    return response?.Exception?.Message;
                }
            }
        }

        public string AlterarSituacao(Guid cupomId, int situacao)
        {
            var cupons = new RootResult();
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/cupom/alterarSituacao/{cupomId}/{situacao}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<ResultService>(xml);
                        return result.Message;
                    }
                    else
                    {
                        return response?.Exception?.Message;
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}