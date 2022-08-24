using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class CategoriaService
    {
        public RootResult ObterCategorias(int limit, int page)
        {
            var categorias = new RootResult();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/Categoria/todas/{Program.TokenVapVupt}/{limit}/{page}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        categorias = JsonConvert.DeserializeObject<RootResult>(xml);

                        return categorias;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return categorias;
            }
        }

        public string Adicionar(Categoria categoria)
        {
            var categoriaJdon = JsonConvert.SerializeObject(categoria);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/Categoria/adiciona", httpContent);

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
        public string Alterar(Categoria categoria)
        {
            var categoriaJdon = JsonConvert.SerializeObject(categoria);
            var httpContent = new StringContent(categoriaJdon, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/Categoria/alterar", httpContent);

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