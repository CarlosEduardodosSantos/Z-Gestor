using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class ProdutoOpcaoService
    {
        public RootResult ObterByProdutoId(string produtoId)
        {
            var opcoes = new RootResult();
            using (var client = new HttpClient())
            {
                var restauranteId = Program.Restaurante.RestauranteId;
                var response = client.GetAsync($"{Program.AddressApi}/api/produtoopcao/obterByGestor/{restauranteId}/{produtoId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        opcoes = JsonConvert.DeserializeObject<RootResult>(xml);

                        return opcoes;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return opcoes;
            }
        }

        public RootResult ObterByTipos()
        {
            var opcoes = new RootResult();
            using (var client = new HttpClient())
            {
                var restauranteId = Program.Restaurante.RestauranteId;
                var response = client.GetAsync($"{Program.AddressApi}/api/produtoopcao/obteTiposByGestor/{restauranteId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        opcoes = JsonConvert.DeserializeObject<RootResult>(xml);

                        return opcoes;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return opcoes;
            }
        }
        public string Adicionar(ProdutoOpcao produtoOpcao)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produtoOpcao);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produtoopcao/adicionar", httpContent);

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

        public string Relacionar(ProdutosOpcaoTipoRelacao produtoOpcao)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produtoOpcao);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produtoopcao/relacionar", httpContent);

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
        public string Alterar(ProdutoOpcao produtoOpcao)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produtoOpcao);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/produtoopcao/alterar", httpContent);

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
        public string Excluir(ProdutoOpcao produtoOpcao)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/produtoopcao/deletar/{produtoOpcao.ProdutosOpcaoId}");

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

        public string AdicionarTipo(ProdutoOpcaoTipo produtoOpcaoTipo)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produtoOpcaoTipo);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produtoopcao/adicionarTipo", httpContent);

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

        public string AlterarTipo(ProdutoOpcaoTipo produtoOpcaoTipo)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produtoOpcaoTipo);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/produtoopcao/alterarTipo", httpContent);

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
        public string ExcluirTipo(ProdutoOpcaoTipo produtoOpcaoTipo)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/produtoopcao/excluirTipo/{produtoOpcaoTipo.ProdutosOpcaoTipoId}");

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

        public string ExcluirRelacao(Guid produtoOpcaoId, int produtoId)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/produtoopcao/deletarRelacao/{produtoOpcaoId}/{produtoId}");

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