using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ZapFood.WinForm.IFoodAutenticate;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Service
{
    public class MerchantService : IDisposable
    {
        private string UrlMerchants = $"{Program.AddressApiIFood}/merchant/v1.0";

        public GenericResult<MerchantModel>  ObterLoja()
        {
            var result = new GenericResult<MerchantModel>();
            var id = Program.MerchantId;
            var url = $"{UrlMerchants}/merchants";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", string.Format("bearer {0}", Program.GetToken()));
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.Result = JsonConvert.DeserializeObject<List<MerchantModel>>(response.Content).Where(t => t.id == id).FirstOrDefault();
                result.Success = true;
                result.Json = response.Content;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.Result = new MerchantModel();
                result.Success = true;
            }
            else
            {
                result.Message = response.StatusDescription;
            }

            return result;

        }

        public UserCodeModel ObterLinkIntegracao()
        {
            var clientId = Program.clientId;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"{Program.AddressApiIFood}/authentication/v1.0/oauth/userCode"))
                {

                    request.Headers.TryAddWithoutValidation("accept", "application/json");
                    request.Content = new StringContent($"clientId={clientId}");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

                    var response = httpClient.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode) return new UserCodeModel();

                    var xml = response.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<UserCodeModel>(xml);
                    
                }


            }
        }
        public MerchantStatusModel StatusLoja()
        {
            if (Program.Merchant == null) return null;
            var merchants = Program.Merchant.id;
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"{UrlMerchants}/merchants/{merchants}/status"))
                {


                    var response = httpClient.SendAsync(request).Result;

                    var xml = response.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<List<MerchantStatusModel>>(xml).FirstOrDefault();
                }

                        
            }
        }

        public bool AlterarStatusLoja(string status, string motivo)
        {
            var merchants = Program.MerchantId;
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PUT"), $"{UrlMerchants}/merchants/{merchants}/statuses"))
                {
                    var values = new Dictionary<string, string>();
                    values.Add("status", status);

                    if (!string.IsNullOrEmpty(motivo))
                        values.Add("reason", motivo);

                    var json = JsonConvert.SerializeObject(values);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }
        public bool RemoverInterrupcao(string status)
        {
            var merchants = Program.MerchantId;
            using (var httpClient = ClientHelper.GetClient(Program.GetToken()))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"{UrlMerchants}/merchants/{merchants}/statuses"))
                {
                    var values = new Dictionary<string, string>();
                    values.Add("status", status);
                    /*
                    if (!string.IsNullOrEmpty(motivo))
                        values.Add("reason", motivo);
                    */
                    var json = JsonConvert.SerializeObject(values);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = httpClient.SendAsync(request).Result;

                    return response.IsSuccessStatusCode;
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
