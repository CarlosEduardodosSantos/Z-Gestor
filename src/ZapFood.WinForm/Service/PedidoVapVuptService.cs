using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Service
{
    public class PedidoVapVuptService : IBaseService
    {
        private readonly string versao = "v3/";
        public PedidoRootModel ObterPedidos()
        {
            var pedidos = new PedidoRootModel();
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(6);

                var response = client.GetAsync($"{Program.AddressApi}/api/{versao}pedido/pendentes/{Program.TokenVapVupt}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var pedido = JsonConvert.DeserializeObject<PedidoRootModel>(xml);

                        return pedido;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return pedidos;
            }
        }

        public List<PollingModel> Obterpolling()
        {
            var pollings = new List<PollingModel>();

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);

                var response = httpClient.GetAsync($"{Program.AddressApi}/api/{versao}pedido/polling/{Program.TokenVapVupt}");

                if (!response.Result.IsSuccessStatusCode) return pollings;

                var xml = response.Result.Content.ReadAsStringAsync().Result;
                pollings = JsonConvert.DeserializeObject<List<PollingModel>>(xml);
            }

            return pollings;
        }

        public PedidoRootModel ObterById(string id)
        {
            var pedidos = new PedidoRootModel();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/{versao}pedido/byId/{id}/{Program.TokenVapVupt}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var pedido = JsonConvert.DeserializeObject<PedidoRootModel>(xml);

                        return pedido;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return null;
            }
        }

        public string ObterJsonPedidoById(string id)
        {
            throw new NotImplementedException();
        }

        public bool AlterarStatusPedido(string rederence, string status, string motivo = "")
        {
            var status_ = string.Empty;

            switch (status)
            {
                case "confirm":
                    status_ = "confirmation";
                    break;
                case "integration":
                    status_ = "integration";
                    break;
                case "requestCancellation":
                    status_ = "cancelled";
                    break;
                case "dispatch":
                    status_ = "dispatch";
                    break;
                case "delivery":
                    status_ = "delivery";
                    break;
                case "readyToPickup":
                    status_ = "readyToDeliver";
                    break;
                default:
                    status_ = "confirmation";
                    break;
            }


            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{Program.AddressApi}/api/v2/pedido/statuss/{rederence}/{status_}/{Program.TokenVapVupt}"))
                {
                    var values = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(motivo))
                    {
                        values.Add("details", motivo);
                        var json = JsonConvert.SerializeObject(values);
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }

        public bool AlterarStatusLoja(string status, string motivo)
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public bool PollingAcknowledgment(List<PollingModel> polling)
        {
            throw new NotImplementedException();
        }

        public MerchantModel StatusLoja()
        {
            throw new NotImplementedException();
        }
    }
}