using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class ProdutoGrupoService
    {
        public RootResult ObterTodas()
        {
            var grupos = new RootResult();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/grupoProduto/obterByRestauranteId/{Program.Restaurante.RestauranteId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        grupos = JsonConvert.DeserializeObject<RootResult>(xml);

                        return grupos;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return grupos;
            }
        }

        public string Adicionar(ProdutoGrupo produtoGrupo)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produtoGrupo);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/grupoProduto/adicionar", httpContent);

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
        public string Alterar(ProdutoGrupo produtoGrupo)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produtoGrupo);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/grupoProduto/alterar", httpContent);

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

        public string Excluir(ProdutoGrupo produtoGrupo)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/grupoProduto/deletar/{produtoGrupo.GupoId}");

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

        public string AdicionarRelacao(RelacaoGrupoCategoriaViewModel relacaoGrupo)
        {
            var categoriaJdon = JsonConvert.SerializeObject(relacaoGrupo);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/grupoProduto/relacionar", httpContent);

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
    }
}