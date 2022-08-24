using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.IFoodAutenticate;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Service
{
    public class PedidoIfoodService : IBaseService
    {
        private string Url = $"{Program.AddressApiIFood}/order/v1.0";
        
        public List<PollingModel> Obterpolling()
        {
            var pollings = new List<PollingModel>();

            using (
               var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
               httpClient.Timeout = TimeSpan.FromMilliseconds(10000);


                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"{Url}/events%3Apolling"))
                {
                    var response = httpClient.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode != HttpStatusCode.NotFound)
                            Program.GetToken(true);

                        return pollings;
                    }
                    

                    var xml = response.Content.ReadAsStringAsync().Result;
                    pollings = JsonConvert.DeserializeObject<List<PollingModel>>(xml) ?? new List<PollingModel>();
                }
            }

            return pollings;
        }

        public bool PollingAcknowledgment(List<PollingModel> pollings)
        {
            
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);

                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{Url}/events/acknowledgment"))
                {

                    var json = JsonConvert.SerializeObject(pollings);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    //request.Content = new StringContent("[{\"id\":\"fb5e8ef6-f17e-4109-a0dc-2d0d5fe36bfa\",\"code\":\"PLC\",\"fullCode\":\"PLACED\",\"orderId\":\"55c677f5-e480-49dc-954b-f412f09c69ea\",\"createdAt\":\"2021-09-03T10:36:10.383Z\"}]");
                   // request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode != HttpStatusCode.NotFound)
                            Program.GetToken(true);

                        return false;
                    }



                    return response.IsSuccessStatusCode;
                }
            }
        }


        public PedidoRootModel ObterById(string id)
        {
            try
            {
                using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"),
                        $"{Url}/orders/{id}"))
                    {
                        var response = httpClient.SendAsync(request).Result;

                        var xml = response.Content.ReadAsStringAsync().Result;

                        return JsonConvert.DeserializeObject<PedidoRootModel>(xml);
                    }

                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string ObterJsonPedidoById(string id)
        {
            try
            {
                using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"),
                        $"{Url}/orders/{id}"))
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

        public bool AlterarStatusPedido(string id, string status, string motivo = "")
        {
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{Url}/orders/{id}/{status}"))
                {
                    var values = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(motivo))
                    {
                        values.Add("reason", motivo);
                        values.Add("cancellationCode", "501");
                        var json = JsonConvert.SerializeObject(values);
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }
       

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
