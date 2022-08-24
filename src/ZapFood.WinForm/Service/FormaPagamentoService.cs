using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class FormaPagamentoService
    {
        public List<FormaPagamento> ObterFormaPagamentos()
        {
            
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/formaPagamento/obterByToken/{Program.TokenVapVupt}/999/1");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var rootFormaPagamentos = JsonConvert.DeserializeObject<RootFormaPagamento>(xml);

                        return rootFormaPagamentos.results;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return new List<FormaPagamento>();
            }
        }
        public List<EspeciePagamento> ObterEspecies()
        {
            var estados = new List<EspeciePagamento>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/formaPagamento/obterEspecies");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        estados = JsonConvert.DeserializeObject<List<EspeciePagamento>>(xml);

                        return estados;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return estados;
            }
        }

        public ResultService Incluir(FormaPagamento formaPagamento)
        {
            
            var formaPagamentoJson = JsonConvert.SerializeObject(formaPagamento);
            var httpContent = new StringContent(formaPagamentoJson, Encoding.UTF8, "application/json");



            using (var client = new HttpClient())
            {
                var response = client.PostAsync($"{Program.AddressApi}/api/formaPagamento/adicionar", httpContent);

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

        public ResultService Excluir(FormaPagamento formaPagamento)
        {

            var id = formaPagamento.FormaPagamentoId.ToString();

            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/formaPagamento/excluir/{id}");

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

        public ResultService Alterar(FormaPagamento formaPagamento)
        {

            var formaPagamentoJson = JsonConvert.SerializeObject(formaPagamento);
            var httpContent = new StringContent(formaPagamentoJson, Encoding.UTF8, "application/json");


            using (var client = new HttpClient())
            {
                var response = client.PutAsync($"{Program.AddressApi}/api/formaPagamento/alterar", httpContent);

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
    }
}