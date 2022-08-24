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
    class ProdutoShiftService
{
        public List<ProdutoShifts> ObterTodas(int produtoId)
        {
            var produtoShifts = new List<ProdutoShifts>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/ProdutoShifts/ByProdId/{produtoId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        produtoShifts = JsonConvert.DeserializeObject<List<ProdutoShifts>>(xml);

                        return produtoShifts;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return produtoShifts;
            }
        }

        public string Adicionar(ProdutoShifts produtoShifts)
        {
            var prodShiftsJson = JsonConvert.SerializeObject(produtoShifts);
            var httpContent = new StringContent(prodShiftsJson, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/ProdutoShifts/", httpContent);

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

        public string Excluir(ProdutoShifts produtoShifts)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/ProdutoShifts/{produtoShifts.ProdutoShiftsGuid}");

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

