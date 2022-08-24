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
    class RatingService

    {
        public List<Rating> ObterTodas()
        {
            var ratings = new List<Rating>();
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"{Program.AddressApi}/api/RestauranteRating/byrestid/{Program.Restaurante.RestauranteId}");
                try
                {

                    if (response.Result.IsSuccessStatusCode)
                    {
                        var xml = response.Result.Content.ReadAsStringAsync().Result;
                        ratings = JsonConvert.DeserializeObject<List<Rating>>(xml);

                        return ratings;

                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return ratings;
            }
        }

        public string Excluir(Rating rating)
        {
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync($"{Program.AddressApi}/api/RestauranteRating/{rating.RestauranteRatingGuid}");

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
