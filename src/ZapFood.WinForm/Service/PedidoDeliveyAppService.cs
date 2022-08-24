using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZapFood.WinForm.DeliveryApp.Utils;
using ZapFood.WinForm.Model;
using ZapFood.WinForm.Model.DeliveryApp;
using ZapFood.WinForm.Model.Ifood;

namespace ZapFood.WinForm.Service
{
    public class PedidoDeliveyAppService : IBaseService
    {
        private string _urlBase = Constants.URL_BASE;

        public GenericResult<IEnumerable<order>> Orders(string token, int status = -1, byte limit = 50)
        {
            var result = new GenericResult<IEnumerable<order>>();
            try
            {
                var url = string.Format("{0}", _urlBase);
                var client = new RestClient(url);
                var request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddParameter("token_account", token);
                request.AddParameter("limit", limit);
                if (status >= 0)
                {
                    request.AddParameter("status", status);
                }
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var resultOrder = JsonConvert.DeserializeObject<result_order>(response.Content);
                    if (resultOrder.code == 200)
                    {
                        result.Result = resultOrder.Orders;
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = string.Join(",", resultOrder.causes.Select(s => s.message));
                    }
                    result.Json = response.Content;
                }
                else
                {
                    result.Message = response.Content + " - " + response.StatusDescription;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public GenericResult<order> Order(string token, string orderId)
        {
            var result = new GenericResult<order>();
            try
            {
                var url = string.Format("{0}/{1}", _urlBase, orderId);
                var client = new RestClient(url);
                var request = new RestRequest(RestSharp.Method.POST);
                request.AddHeader("Accept", "application/json");
                request.AddParameter("token_account", token);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var resultOrder = JsonConvert.DeserializeObject<result_order>(response.Content);
                    if (resultOrder.code == 200)
                    {
                        result.Result = resultOrder.Order;
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = string.Join(",", resultOrder.causes.Select(s => s.message));
                    }

                    result.Json = response.Content;
                }
                else
                {
                    result.Message = response.Content + " - " + response.StatusDescription;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public GenericResult<order> Status(string token, string orderId, byte status)
        {
            var result = new GenericResult<order>();
            try
            {
                var url = string.Format("{0}/{1}", _urlBase, orderId);
                var client = new RestClient(url);
                var request = new RestRequest(RestSharp.Method.PUT);
                request.AddHeader("Accept", "application/json");
                request.AddParameter("token_account", token);
                request.AddParameter("status", status);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var resultOrder = JsonConvert.DeserializeObject<result_order>(response.Content);
                    if (resultOrder.code == 200)
                    {
                        result.Result = resultOrder.Order;
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = string.Join(",", resultOrder.causes.Select(s => s.message));
                    }

                    result.Json = response.Content;
                }
                else
                {
                    result.Message = response.Content + " - " + response.StatusDescription;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public List<PollingModel> Obterpolling()
        {
            throw new NotImplementedException();
        }

        public bool PollingAcknowledgment(List<PollingModel> polling)
        {
            throw new NotImplementedException();
        }

        public PedidoRootModel ObterById(string id)
        {
            throw new NotImplementedException();
        }

        public string ObterJsonPedidoById(string id)
        {
            throw new NotImplementedException();
        }

        public bool AlterarStatusPedido(string rederence, string status, string motivo = "")
        {
            var result = new GenericResult<order>();
            try
            {
                var token = Program.TokenDeliveryApp;
                var status_ = byte.MinValue;

                switch (status)
                {
                    case "confirm":
                        status_ = (byte)DeliveryApp.Enum.OrderStatus.Confirmado;
                        break;
                    case "requestCancellation":
                        status_ = (byte)DeliveryApp.Enum.OrderStatus.Cancelado;
                        break;
                    case "dispatch":
                        status_ = (byte)DeliveryApp.Enum.OrderStatus.Enviado;
                        break;
                    case "delivery":
                        status_ = (byte)DeliveryApp.Enum.OrderStatus.Entregue;
                        break;
                    case "readyToPickup":
                        status_ = (byte)DeliveryApp.Enum.OrderStatus.Enviado;
                        break;
                    default:
                        status_ = (byte)DeliveryApp.Enum.OrderStatus.Confirmado;
                        break;
                }
 
                var url = string.Format("{0}/{1}", _urlBase, rederence);
                var client = new RestClient(url);
                var request = new RestRequest(RestSharp.Method.PUT);
                request.AddHeader("Accept", "application/json");
                request.AddParameter("token_account", token);
                request.AddParameter("status", status_);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var resultOrder = JsonConvert.DeserializeObject<result_order>(response.Content);
                    if (resultOrder.code == 200)
                    {
                        result.Result = resultOrder.Order;
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = string.Join(",", resultOrder.causes.Select(s => s.message));
                    }

                    result.Json = response.Content;
                }
                else
                {
                    result.Message = response.Content + " - " + response.StatusDescription;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result.Success;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
