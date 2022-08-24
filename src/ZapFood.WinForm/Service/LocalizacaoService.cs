using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class LocalizacaoService
    {
        public List<Estado> ObterEstados()
        {
            var estados = new List<Estado>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/localizacao/estados");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        estados = JsonConvert.DeserializeObject<List<Estado>>(xml);

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

        public List<Cidade> ObterCidades(string estado)
        {
            var cidades = new List<Cidade>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/localizacao/cidades/{estado}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        cidades = JsonConvert.DeserializeObject<List<Cidade>>(xml);

                        return cidades;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return cidades;
            }
        }

        public List<Bairro> ObterBairros(string estado, string cidade)
        {
            var bairros = new List<Bairro>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/localizacao/bairros/{estado}/{cidade}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        bairros = JsonConvert.DeserializeObject<List<Bairro>>(xml);

                        return bairros;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return bairros;
            }
        }
    }
}