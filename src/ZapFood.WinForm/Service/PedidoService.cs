using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ZapFood.WinForm.Data.Entity;
using ZapFood.WinForm.Data.Repository;
using ZapFood.WinForm.IFoodAutenticate;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;
using RootObject = ZapFood.WinForm.Model.RootObject;

namespace ZapFood.WinForm.Service
{
    public class PedidoService : IDisposable
    {
        public ICollection<Pedido> ObterPedidos(int loja, string addressApi)
        {
            var pedidos = new List<Pedido>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(addressApi + "");

                if (response.Result.IsSuccessStatusCode)
                {
                    var xml = response.Result.Content.ReadAsStringAsync().Result;

                    var serializer = new XmlSerializer(typeof(Pedidos));

                    var reader = new System.IO.StringReader(xml);

                    
                    var result = (Pedidos)serializer.Deserialize(reader);

                    return result.pedido;

                    //var pedido = JsonConvert.DeserializeObject<object>(result);
                }
                return pedidos;
            }
        }

        public RootObject ObterPedidosZip(int limit, int page)
        {
            var pedidos = new RootObject();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/pedido/pendentes/{Program.TokenVapVupt}/{limit}/{page}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        var pedido = JsonConvert.DeserializeObject<RootObject>(xml);

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

        public void AceitarPedido(string json)
        {
            var body = new StringContent(json, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync($"{Program.AddressApi}/api/pedido/alterar", body);

                if (response.Result.IsSuccessStatusCode)
                {
                }
            }
        }

        public void Dispose()
        {
            
        }

        public List<PedidoModelIFood> ObterPedidosIfood(List<PedidoVapVupt> eventos)
        {
            var pedidos = new List<PedidoModelIFood>();
            PedidoModelIFood pedido = null;

            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                foreach (var evento in eventos)
                {
                    if (!string.IsNullOrEmpty(evento.FileJson))
                    {
                        try
                        {
                            pedido = JsonConvert.DeserializeObject<PedidoModelIFood>(evento.FileJson);
                            
                        }
                        catch
                        {
                            //Ignore
                        }
                    }
                    else
                    {
                        using (var request = new HttpRequestMessage(new HttpMethod("GET"),
                            $"https://pos-api.ifood.com.br/v1.0/orders/{evento.PedidoId}"))
                        {
                            var response = httpClient.SendAsync(request).Result;

                            if (!response.IsSuccessStatusCode) continue;

                            var xml = response.Content.ReadAsStringAsync().Result;
                            pedido = JsonConvert.DeserializeObject<PedidoModelIFood>(xml);
           
                        }
       
                    }
                    if (pedido == null) continue;
                    pedido.sitEvent = ((SituacaoVapVuptEnum)evento.Situacao).ToString();
                    pedidos.Add(pedido);
                }
                
            }

            return pedidos;
        }
        public string ObterJsonPedidosIfoodById(string pedidoId)
        {
            try
            {
                using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"),
                        $"https://pos-api.ifood.com.br/v1.0/orders/{pedidoId}"))
                    {
                        var response = httpClient.SendAsync(request).Result;

                        return response.Content.ReadAsStringAsync().Result;

                    }

                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public PedidoModelIFood ObterPedidosIfoodById(string pedidoId)
        {
            try
            {
                using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"),
                        $"https://pos-api.ifood.com.br/v1.0/orders/{pedidoId}"))
                    {
                        var response = httpClient.SendAsync(request).Result;

                        var xml = response.Content.ReadAsStringAsync().Result;

                        return JsonConvert.DeserializeObject<PedidoModelIFood>(xml);
                    }

                }
            }
            catch
            {
                return null;
            }
        }


        public List<PollingModel> Obterpolling()
        {
            var pollings = new List<PollingModel>();

            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://pos-api.ifood.com.br/v1.0/events%3Apolling"))
                {
                    var response = httpClient.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode) return pollings;

                    var xml = response.Content.ReadAsStringAsync().Result;
                    pollings = JsonConvert.DeserializeObject<List<PollingModel>>(xml);
                }
            }

            return pollings;
        }

        public bool AceitarPedidoIfood(string rederence, string status, string motivo = "")
        {

            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://pos-api.ifood.com.br/v1.0/orders/{rederence}/statuses/{status}"))
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
                    /*
                    if (!response.IsSuccessStatusCode) return;

                    var xml = response.Content.ReadAsStringAsync().Result;
                    pollings = JsonConvert.DeserializeObject<List<PollingModel>>(xml);*/
                }
            }
        }
        public bool Acknowledgment(string dataJson)
        {
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://pos-api.ifood.com.br/v1.0/events/acknowledgment"))
                {
                    request.Content = new StringContent(dataJson, Encoding.UTF8, "application/json");

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }
        public bool StatusLoja(string status, string motivo)
        {
            var merchants = Program.MerchantId;
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PUT"), $"https://pos-api.ifood.com.br/v1.0/merchants/{merchants}/statuses"))
                {
                    var values = new Dictionary<string, string>();
                    values.Add("status", status);

                    if (!string.IsNullOrEmpty(motivo))
                        values.Add("reason", motivo);

                    var json = JsonConvert.SerializeObject(values);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    //request.Content = new FormUrlEncodedContent(values); //StringContent($"status={status}",Encoding.UTF8, "application/x-www-form-urlencoded");

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }
        public bool StatusProduto(string status, string externalCode)
        {
            var merchants = Program.MerchantId;
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://pos-api.ifood.com.br/v1.0/merchants/{merchants}/skus/{externalCode}"))
                {
                    var values = new Dictionary<string, string>();
                    values.Add("status", status);
                    var json = JsonConvert.SerializeObject(values);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }
        public bool AtualizaPrecoProduto(decimal prices, string externalCode)
        {
            var merchants = Program.MerchantId;
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://pos-api.ifood.com.br/v1.0/skus/{externalCode}/prices"))
                {
//                    string[] array = new string[] { merchants };
                    List<string> array = new List<string>();
                    array.Add(merchants);

                    var marchantIds = JsonConvert.SerializeObject(array);
                    var values = new Dictionary<string, string>();
                    values.Add("merchantIds", marchantIds);
                    values.Add("externalCode", externalCode);
                    values.Add("price", prices.ToString());
                    values.Add("startDate", Funcoes.ConvertDateTimeFuso(DateTime.Now.AddHours(2).ToString()));

                    var json = JsonConvert.SerializeObject(values).Replace("\\", "");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }
    }

    [XmlRoot("pedidos")]
    public class Pedidos
    {
        [XmlElement("pedido")]
        public List<Pedido> pedido { get; set; }
    }
}