using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class ProdutoService
    {
        public ProdutoRoot ObterProdutos(int limit, int page)
        {
            var produtos = new ProdutoRoot();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/produto/produtoscadastro/{Program.TokenVapVupt}/{limit}/{page}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        produtos = JsonConvert.DeserializeObject<ProdutoRoot>(xml);

                        return produtos;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return produtos;
            }
        }
        public string Adicionar(Produto produto)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produto);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");
            
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produto/adicionar", httpContent);

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

        public Produto Copiar(Produto produto)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produto);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produto/copiar", httpContent);

                if (response.Result.IsSuccessStatusCode)
                {
                    var xml = response.Result.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<Produto>(xml);
                    return result;
                }
                else
                {
                    throw new Exception("Erro ao copiar o produto\nErro:" + response.Result.StatusCode.ToString());
                }
            }
        }
        public string Excluir(Produto produto)
        {
            var categoriaJdon = JsonConvert.SerializeObject(produto);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produto/excluir", httpContent);

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
        public List<Complemento> ObterComplementos(int grupoId)
        {
            var complementos = new List<Complemento>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/produto/complementos/{grupoId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        complementos = JsonConvert.DeserializeObject<List<Complemento>>(xml);

                        return complementos;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return complementos;
            }
        }
        public string AddComplementos(Complemento complemento)
        {
            complemento.TokenRestaurante = Program.TokenVapVupt;
            var categoriaJdon = JsonConvert.SerializeObject(complemento);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");

            

            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produto/addComplemento", httpContent);

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
        public string RemoveComplementos(Complemento complemento)
        {
            complemento.TokenRestaurante = Program.TokenVapVupt;
            var categoriaJdon = JsonConvert.SerializeObject(complemento);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");



            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/produto/removerComplemento", httpContent);

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