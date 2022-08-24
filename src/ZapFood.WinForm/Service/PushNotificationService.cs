using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.Service
{
    public class PushNotificationService
    {
        public ResultService Enviar(string cabecalho, string message)
        {
            var restauranteId = Program.Restaurante.Token;

            var data = new
            {
                RestauranteToken = restauranteId,
                Cabecalho = cabecalho,
                Mensagem = message
            };

            var cadaJson = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(cadaJson, Encoding.UTF8, "application/json");



            using (var httpClient = new HttpClient())
            {
                var response = httpClient.PostAsync($"{Program.AddressApi}/api/notificacao/enviar", httpContent);
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