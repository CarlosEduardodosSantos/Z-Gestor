using System;
using ZapFood.WinForm.Model;

namespace ZapFood.WinForm.DeliveryApp.Utils
{
    public class ConvertApp
    {
        public static PedidoRootModel PedidoParse(Model.DeliveryApp.order order)
        {
            var pedidoRoot = new PedidoRootModel();
            var dataPedido = System.DateTime.Parse(order.date);
            pedidoRoot.id = order.id.ToString();
            pedidoRoot.createdAt = dataPedido;
            pedidoRoot.orderType = order.forma_entrega == 1 ? "DELIVERY" : "TAKEOUT";
            pedidoRoot.shortReference = order.order_number.ToString();

            if (order.is_scheduled)
            {
                pedidoRoot.orderTiming = order.is_scheduled ? "SCHEDULED" : "";
                pedidoRoot.schedule = new Schedule();
                var dataAgendamento = DateTime.Now;
                DateTime.TryParse(order.scheduled_at, out dataAgendamento);

                pedidoRoot.schedule.deliveryDateTimeStart = dataAgendamento;
            }
            

            pedidoRoot.observations = order.notes;

            pedidoRoot.customer = new Customer()
            {
                name = order.name,
                documentNumber = order.cpf_in_note
            };
            pedidoRoot.customer.phone = new Phone()
            {
                number = order.phone
            };
            pedidoRoot.delivery = new Delivery()
            {
                deliveryDateTime = dataPedido.AddHours(3).AddMinutes(60),
                deliveryAddress = new Deliveryaddress()
                {
                    formattedAddress = order.street,
                    streetNumber = order.number,
                    neighborhood = order.neighborhood,
                    postalCode = order.cep,
                    city = order.city,
                    complement = order.complement,
                    reference = order.reference_point
                }
            };

            pedidoRoot.total = new Total()
            {
                deliveryFee = float.Parse(order.tax.ToString()),
                subTotal = float.Parse(order.sub_total.ToString()),
                benefits = float.Parse(order.total_discount.ToString())

            };
            pedidoRoot.benefits = new System.Collections.Generic.List<Benefit>();
            pedidoRoot.benefits.Add(new Benefit
            {
                value = float.Parse(order.total_discount.ToString())
            });
            pedidoRoot.deliveryFee = order.tax;
            pedidoRoot.payments = new Payments();
            pedidoRoot.payments.methods = new Method[1];
            pedidoRoot.payments.methods[0] = new Method();
            pedidoRoot.payments.methods[0].cash = new Cash() { changeFor = order.troco };
            var method = order.payment_method.Trim().ToUpper();

            switch (method)
            {
                case "CRÉDITO":
                    pedidoRoot.payments.methods[0].method = "CREDIT";
                    break;
                case "DÉBITO":
                    pedidoRoot.payments.methods[0].method = "CREDIT";
                    break;
                case "DINHEIRO":
                    pedidoRoot.payments.methods[0].method = "CASH";
                    break;
                default:
                    pedidoRoot.payments.methods[0].method = "CREDIT";
                    break;
            }
            
            pedidoRoot.payments.methods[0].value = order.total;
            pedidoRoot.payments.methods[0].type = order.payment_online ? "ONLINE" : "";
            pedidoRoot.items = new System.Collections.Generic.List<Item>();
            foreach (var item in order.ItemOrder)
            {
                var pedidoItem = new Item()
                {
                    externalCode = item.variacao_ref,
                    name = $"{item.title} {item.name}",
                    quantity = item.quantity,
                    unitPrice = item.price,
                    price = item.total,
                    totalPrice = item.total
                };

                if (item.ComplementCategories != null)
                {
                    pedidoItem.options = new Option[item.ComplementCategories.Count];

                    for (int i = 0; i < item.ComplementCategories.Count; i++)
                    {
                        var option = item.ComplementCategories[i];
                        foreach (var complement in option.Complements)
                        {
                            pedidoItem.options[i] = new Option()
                            {
                                name = complement.title,
                                externalCode = complement._ref,
                                quantity = complement.quantity,
                                unitPrice = complement.price_un,
                                price = complement.total
                            };

                        }



                    }
                }

                pedidoRoot.items.Add(pedidoItem);
            }
            return pedidoRoot;
        }
    }
}
