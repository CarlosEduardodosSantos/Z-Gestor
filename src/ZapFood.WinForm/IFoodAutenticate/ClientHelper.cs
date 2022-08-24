using System;
using RestSharp;
using System.Text;
using Newtonsoft.Json;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.IFoodAutenticate
{
    public static class ClientHelper
    {
        // Basic auth
        public static GenericResult<AccessTokenModelView> GetToken(string username, string password)
        {
            var authorizationCode = Program.AuthorizationCode;
            var authorizationCodeVerifier = Program.AuthorizationCodeVerifier;
     

            var result = new GenericResult<AccessTokenModelView>();

            var client = new RestClient($"{Program.AddressApiIFood}/authentication/v1.0/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddParameter("clientId", username);
            request.AddParameter("clientSecret", password);
            if (Program.TipoAplicativoIfood == TipoAplicativoEnumModel.Distribuído)
            {
                request.AddParameter("grantType", "authorization_code");
                request.AddParameter("authorizationCode", authorizationCode);
                request.AddParameter("authorizationCodeVerifier", authorizationCodeVerifier);
            }
            else
            {
                request.AddParameter("grantType", "client_credentials");
                request.AddParameter("authorizationCode", "");
                request.AddParameter("authorizationCodeVerifier", "");
            }
 
            request.AddParameter("refreshToken", "");
            IRestResponse responseToken = client.Execute(request);

            if (responseToken.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.Result = JsonConvert.DeserializeObject<AccessTokenModelView>(responseToken.Content);
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Message = responseToken.StatusDescription;
            }

            return result;

        }

        public static GenericResult<AccessTokenModelView> RefreshToken(string username, string password, string accesstoken)
        {

            var result = new GenericResult<AccessTokenModelView>();

            var client = new RestClient($"{Program.AddressApiIFood}/authentication/v1.0/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddParameter("clientId", username);
            request.AddParameter("clientSecret", password);
            request.AddParameter("grantType", "refresh_token");
            request.AddParameter("authorizationCode", "");
            request.AddParameter("authorizationCodeVerifier", "");
            request.AddParameter("refreshToken", accesstoken);

            IRestResponse responseToken = client.Execute(request);

            if (responseToken.StatusCode == System.Net.HttpStatusCode.OK)
            {
                result.Result = JsonConvert.DeserializeObject<AccessTokenModelView>(responseToken.Content);
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.Message = responseToken.StatusDescription;
            }

            return result;

        }

        public static System.Net.Http.HttpClient GetClient(string token)
        {
            var authValue = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var client = new System.Net.Http.HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
                //Set some other client defaults like timeout / BaseAddress
            };
            return client;
        }

    }
}