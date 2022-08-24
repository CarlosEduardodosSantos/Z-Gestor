using System;
using System.Collections.Generic;
using System.Linq;
using ZapFood.WinForm.Data.Repository;

namespace ZapFood.WinForm.Model
{
    public class PedidoRootModel
    {
        public string id { get; set; }
        public string reference { get; set; }
        public string shortReference { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime DataPedido => aplicacao == AplicacaoEnum.Ifood ? createdAt.AddHours(-3) : createdAt;
        public Schedule schedule { get; set; }
        public bool isSchedule => orderTiming == "SCHEDULED";
        public string orderTiming { get; set; }
        public string orderType { get; set; }
        public string displayId { get; set; }
        public Merchant merchant { get; set; }
        public Customer customer { get; set; }
        public List<Item> items { get; set; }
        public string salesChannel { get; set; }
        public Total total { get; set; }
        public decimal valorTotal => ((decimal)total.subTotal + (decimal)total.deliveryFee + valorServico) - VoucherDesconto;
        public decimal valorServico => ValorServico();
        public Payments payments { get; set; }
        public decimal subTotal => payments.methods != null ? payments.methods.Sum(t => t.value) : 0;
        public decimal totalPrice { get; set; }
        public decimal deliveryFee { get; set; }
        public decimal totalVoucher => benefits != null ? (decimal)benefits.Sum(t => t.value) : 0;
        public decimal VoucherDesconto => VarificaValorVoucher();
        public Delivery delivery { get; set; }
        public DateTime deliveryDateTime { get; set; }
        public DateTime DataEntrega => orderType == "DELIVERY" ? delivery.deliveryDateTime.AddHours(-3) : DataPedido.AddMinutes(30);
        public DateTime preparationStartDateTime { get; set; }
        public DateTime DataPepraracao => preparationStartDateTime != DateTime.MinValue ? preparationStartDateTime.AddHours(-1) : DateTime.Now;
        public Localizer localizer { get; set; }
        public int preparationTimeInSeconds { get; set; }
        public bool isTest { get; set; }
        public List<Benefit> benefits { get; set; }
        public string tipoCupom => ObterTipoCupom();
        public string sitEvent { get; set; }
        public string observations { get; set; }
        public AplicacaoEnum aplicacao { get; set; }
        public string vendaId { get; set; }
        public string senhaId { get; set; }
        public string extraInfo { get; set; }
        public string qrcodePix { get; set; }
        public Additionalfee[] additionalFees { get; set; }
        public string TipoPagamento => VerificaTipoPagamento();

        private string VerificaTipoPagamento()
        {
            var isPagto = payments.methods.Where(t => t.type == "ONLINE").Any();
            return isPagto ? $"****PAGO ONLINE VIA {aplicacao}****" : "***RECEBER NA ENTREGA****";
        }

        private decimal ValorServico()
        {
            if (additionalFees == null) return 0;


            return additionalFees.Sum(t => t.value);
        }
        private decimal VarificaValorVoucher()
        {
            if (totalVoucher == 0) return 0;

            return tipoCupom == "IFOOD" ? 0 : totalVoucher;
        }
        private string ObterTipoCupom()
        {
            if (benefits == null) return "";

            foreach (var benefits in benefits)
            {
                if (string.IsNullOrEmpty(benefits.target)) return "";

                if (benefits.target == "DELIVERY_FEE") return "IFOOD";

                foreach (var item in benefits.sponsorshipValues)
                {
                    if (item.value > 0)
                        return item.name == "IFOOD" ? "IFOOD" : "RESTAURANTE";
                }
            }

            return "";
            //benefits.FirstOrDefault(t => t.sponsorshipValues[0].value > 0).target = "" ? "IFOOD" : "RESTAURANTE";
        }

    }

    public class Merchant
    {
        public string id { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
    }
    public class Schedule
    {
        public DateTime deliveryDateTimeStart { get; set; }
        public DateTime deliveryDateTimeEnd { get; set; }
    }
    public class Address
    {
        public string formattedAddress { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string neighborhood { get; set; }
        public string streetName { get; set; }
        public string streetNumber { get; set; }
        public string postalCode { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string documentNumber { get; set; }
        public Phone phone { get; set; }
        public int ordersCountOnMerchant { get; set; }
    }

    public class Phone
    {
        public string number { get; set; }
        public string localizer { get; set; }
        public DateTime localizerExpiration { get; set; }
    }
    public class Delivery
    {
        public string mode { get; set; }
        public string deliveredBy { get; set; }
        public DateTime deliveryDateTime { get; set; }
        public Deliveryaddress deliveryAddress { get; set; }
    }
    public class Deliveryaddress
    {
        public string formattedAddress { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public Coordinates coordinates { get; set; }
        public string neighborhood { get; set; }
        public string streetName { get; set; }
        public string streetNumber { get; set; }
        public string postalCode { get; set; }
        public string reference { get; set; }
        public string complement { get; set; }
    }

    public class Coordinates
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
    }

    public class Localizer
    {
        public string id { get; set; }
    }

    public class Payment
    {
        public string name { get; set; }
        public string code { get; set; }
        public decimal value { get; set; }
        public bool prepaid { get; set; }
        public string collector { get; set; }
        public string issuer { get; set; }
        public string transaction { get; set; }
        public string authorizationCode { get; set; }
        public string externalCode { get; set; }
        public decimal changeFor { get; set; }
    }

    public class Payments
    {
        public string name { get; set; }
        public float prepaid { get; set; }
        public decimal pending { get; set; }
        public decimal value { get; set; }
        public decimal changeFor { get; set; }
        public Method[] methods { get; set; }
        public decimal TrocoPara => methods != null ? methods.Sum(t => t.cash != null ? t.cash.changeFor : 0) : changeFor;
    }

    public class Method
    {
        public decimal value { get; set; }
        public string currency { get; set; }
        public string method { get; set; }
        public string formaPagamento => method == "CREDIT" ? "CREDITO" : method == "CASH" ? "DINHEIRO" : "DEBITO";
        public string type { get; set; }
        public Cash cash { get; set; }
        public bool prepaid { get; set; }
    }

    public class Cash
    {
        public decimal changeFor { get; set; }
    }

    public class Item
    {
        public int index { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string tamanho { get; set; }

        public int quantity { get; set; }
        public decimal unitPriceCalc => Calculaunitario();
        public decimal price { get; set; }
        public decimal unitPrice { get; set; }
        public decimal optionsPrice { get; set; }
        public decimal totalPrice { get; set; }
        public decimal subItemsPrice { get; set; }

        public decimal totalItem => unitPrice > 0 ? (unitPrice + optionsPrice) * quantity : optionsPrice;
        public decimal discount { get; set; }
        public decimal addition { get; set; }
        private string _externalCode { get; set; }
        public Option[] options { get; set; }
        public string externalCode { get; set; }
        public string DescricaoImpressao { get; set; }
        public string externalCodeCalc
        {
            get
            {
                if (!Funcoes.Val_NumeroKey(externalCode))
                    return string.Empty;
                if (_externalCode != "999999") return externalCode;

                if (options.Count() == 0) return externalCode;

                return options[0].externalCode;
            }
            set { _externalCode = value; }
        }
        public string observations { get; set; }
        public virtual bool isRelacao => ProdutoRelacionado();
        public string observacao => ObterObservacao();
        public string observacao999998 => ObterObservacao999998();
        public string NamePrincipal => namePrincipal();
        public List<Subitem> subItems { get; set; }
        public List<SubMeioMeio> subMeioMeios { get; set; }

        private decimal Calculaunitario()
        {
            if (quantity > 1 && optionsPrice == totalPrice)
                return (unitPrice + optionsPrice) / quantity;

            if (!Funcoes.Val_NumeroKey(externalCode))
                return unitPrice + optionsPrice;
            if (externalCode != "999999") return unitPrice + optionsPrice;

            if (options.Count() == 0) return unitPrice + optionsPrice;

            return options.Sum(t => t.price);
        }
        private string ObterObservacao()
        {

            string obs = String.Empty;
            if (options == null) return observations ?? string.Empty;

            foreach (var subitem in options)
            {
                if (string.IsNullOrEmpty(subitem.externalCode))
                    obs += $"\n{subitem.name} ";
                if (subitem.externalCode == "999999")
                    obs += $"\n{subitem.name} ";
                if (subitem.externalCode == "999998")
                    obs += $"\n{subitem.name} ";
            }

            if (!string.IsNullOrEmpty(observations))
                obs += $" \n{observations}";

            return obs;
        }
        private string ObterObservacao999998()
        {
            var obs = string.Empty;
            if (options == null) return observations;
            string separator = price > 0 && subItemsPrice > 0 ? $" {quantity } " : " | ";
            return options.Where(t => t.externalCode == "999998").Aggregate(obs, (current, subItem) => current + (separator + subItem.name)) + " " + observations;
        }

        private string namePrincipal()
        {
            var result = name;

            if (subMeioMeios != null)
            {
                foreach (var submeiomeio in subMeioMeios)
                {
                    result += $"\n 1\\{subMeioMeios.Count} - {submeiomeio.name}";
                }

            } 

            if (options == null) return result += $"\n{observations}";

            foreach (var subitem in options)
            {
                if (subitem.confirmedItem) continue;

                //var quantidadeSub = subitem.quantity * quantity;
                var quantidadeSub = quantity;

                result += $"\n + { quantidadeSub } x {subitem.name}";
            }

            if (!string.IsNullOrEmpty(observations))
                result += $"\n{observations}";

            return result;
        }

        private bool ProdutoRelacionado()
        {
            return Program.TipoDatabase != TipoDatabaseEnum.SQLSever || new ProdutoRepository().VerificaRelacao(externalCodeCalc);
        }
    }
    public class Option
    {
        public int index { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal addition { get; set; }
        public decimal price { get; set; }
        public string externalCode { get; set; }
        public bool confirmedItem { get; set; }
    }

    public class Subitem
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal totalPrice { get; set; }
        public decimal discount { get; set; }
        public decimal addition { get; set; }
        public string externalCode { get; set; }
    }

    public class SubMeioMeio
    {
        public string produtoId { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public decimal totalPrice { get; set; }
        public decimal discount { get; set; }
        public decimal addition { get; set; }
        public string externalCode { get; set; }
        public List<complementosMeioMeio> complementosMeioMeio { get; set; }
    }
    public class Benefit
    {
        public float value { get; set; }
        public Sponsorshipvalue[] sponsorshipValues { get; set; }
        public string target { get; set; }
    }

    public class Sponsorshipvalue
    {
        public string name { get; set; }
        public float value { get; set; }
    }

    public class Total
    {
        public float subTotal { get; set; }
        public float deliveryFee { get; set; }
        public float benefits { get; set; }
        public float orderAmount { get; set; }
        public float additionalFees { get; set; }
    }

    public class Additionalfee
    {
        public string type { get; set; }
        public decimal value { get; set; }
    }

}