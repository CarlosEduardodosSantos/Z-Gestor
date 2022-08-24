using System;
using System.Collections.Generic;
using System.Linq;
using ZapFood.WinForm.Data.Repository;

namespace ZapFood.WinForm.Model.Ifood
{

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

    public class Merchant
    {
        public string id { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
    }

    public class Payment
    {
        public string name { get; set; }
        public string code { get; set; }
        public decimal value { get; set; }
        public bool prepaid { get; set; }
        public string externalCode { get; set; }
        public decimal changeFor { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string taxPayerIdentificationNumber { get; set; }
        public int ordersCountOnRestaurant { get; set; }
    }

    public class SubItem
    {
        public string name { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public decimal totalPrice { get; set; }
        public decimal discount { get; set; }
        public decimal addition { get; set; }
        public string externalCode { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
        public decimal subItemsPrice { get; set; }
        public decimal totalPrice { get; set; }
        public decimal discount { get; set; }
        public decimal addition { get; set; }
        public string externalCode { get; set; }
        public string observations { get; set; }
        public List<SubItem> subItems { get; set; }
        //public virtual bool isRelacao => new ProdutoRepository().VerificaRelacao(externalCode);
        public virtual bool isRelacao => true;
        public string observacao => ObterObservacao();
        public string observacao999998 => ObterObservacao999998();

        private string ObterObservacao()
        {
            string obs = String.Empty; 
            if (subItems == null)return observations;
            string separator = price > 0 && subItemsPrice > 0 ? " + " : " | ";
            return subItems.Aggregate(obs, (current, subItem) => current + (separator + subItem.name)) + " " + observations;
        }
        private string ObterObservacao999998()
        {
            string obs = String.Empty;
            if (subItems == null) return observations;
            string separator = price > 0 && subItemsPrice > 0 ? " + " : " | ";
            return subItems.Where(t => t.externalCode == "999998").Aggregate(obs, (current, subItem) => current + (separator + subItem.name)) + " " + observations;
        }
    }

    public class Coordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class DeliveryAddress
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

    public class PedidoModelIFood
    {
        public string id { get; set; }
        public string reference { get; set; }
        public string shortReference { get; set; }
        private DateTime _createdAt;
        public DateTime createdAt {
            get => _createdAt.AddHours(-3);
            set => _createdAt = value;
        }
        public string type { get; set; }
        public Merchant merchant { get; set; }
        public List<Payment> payments { get; set; }
        public Customer customer { get; set; }
        public List<Item> items { get; set; }
        public decimal subTotal { get; set; }
        public decimal totalPrice { get; set; }
        public decimal deliveryFee { get; set; }
        public DeliveryAddress deliveryAddress { get; set; }
        public DateTime deliveryDateTime { get; set; }
        public string sitEvent { get; set; }
        
    }
}