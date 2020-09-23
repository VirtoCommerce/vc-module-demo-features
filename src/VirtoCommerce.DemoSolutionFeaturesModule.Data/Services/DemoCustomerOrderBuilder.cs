using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.OrdersModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using LineItem = VirtoCommerce.OrdersModule.Core.Model.LineItem;
using Shipment = VirtoCommerce.OrdersModule.Core.Model.Shipment;
using ShipmentItem = VirtoCommerce.OrdersModule.Core.Model.ShipmentItem;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services
{
    public class DemoCustomerOrderBuilder : CustomerOrderBuilder
    {
        public DemoCustomerOrderBuilder(ICustomerOrderService customerOrderService) : base(customerOrderService)
        {
        }

        protected override CustomerOrder ConvertCartToOrder(ShoppingCart cart)
        {
            var retVal = AbstractTypeFactory<CustomerOrder>.TryCreateInstance();
            retVal.ShoppingCartId = cart.Id;
            retVal.PurchaseOrderNumber = cart.PurchaseOrderNumber;
            retVal.Comment = cart.Comment;
            retVal.Currency = cart.Currency;
            retVal.ChannelId = cart.ChannelId;
            retVal.CustomerId = cart.CustomerId;
            retVal.CustomerName = cart.CustomerName;
            retVal.DiscountAmount = cart.DiscountAmount;
            retVal.OrganizationId = cart.OrganizationId;
            retVal.StoreId = cart.StoreId;
            retVal.TaxPercentRate = cart.TaxPercentRate;
            retVal.TaxType = cart.TaxType;
            retVal.LanguageCode = cart.LanguageCode;

            retVal.Status = "Unpaid";

            var cartLineItemsMap = new Dictionary<string, LineItem>();
            if (cart.Items != null)
            {
                retVal.Items = new List<LineItem>();
                foreach (var cartLineItem in cart.Items)
                {
                    var orderLineItem = ToOrderModel(cartLineItem);
                    retVal.Items.Add(orderLineItem);
                    cartLineItemsMap.Add(cartLineItem.Id, orderLineItem);
                }
            }
            if (cart.Discounts != null)
            {
                retVal.Discounts = cart.Discounts.Select(ToOrderModel).ToList();
            }

            if (cart.Addresses != null)
            {
                retVal.Addresses = cart.Addresses.Select(ToOrderModel).ToList();
            }

            if (cart.Shipments != null)
            {
                retVal.Shipments = new List<Shipment>();
                foreach (var cartShipment in cart.Shipments)
                {
                    var shipment = ToOrderModel(cartShipment);
                    if (!cartShipment.Items.IsNullOrEmpty())
                    {
                        shipment.Items = new List<ShipmentItem>();
                        foreach (var cartShipmentItem in cartShipment.Items)
                        {
                            var shipmentItem = ToOrderModel(cartShipmentItem);
                            if (cartLineItemsMap.ContainsKey(cartShipmentItem.LineItemId))
                            {
                                shipmentItem.LineItem = cartLineItemsMap[cartShipmentItem.LineItemId];
                                shipment.Items.Add(shipmentItem);
                            }
                        }
                    }
                    retVal.Shipments.Add(shipment);
                }
                //Add shipping address to order
                retVal.Addresses.AddRange(retVal.Shipments.Where(x => x.DeliveryAddress != null).Select(x => x.DeliveryAddress));

            }
            if (cart.Payments != null)
            {
                retVal.InPayments = new List<PaymentIn>();
                foreach (var payment in cart.Payments)
                {
                    var paymentIn = ToOrderModel(payment);
                    paymentIn.CustomerId = cart.CustomerId;
                    retVal.InPayments.Add(paymentIn);
                    if (payment.BillingAddress != null)
                    {
                        retVal.Addresses.Add(ToOrderModel(payment.BillingAddress));
                    }
                }
            }

            if (cart.DynamicProperties != null)
            {
                retVal.DynamicProperties = cart.DynamicProperties.Select(ToOrderModel).ToList();
            }

            //Save only disctinct addresses for order
            retVal.Addresses = retVal.Addresses.Distinct().ToList();
            foreach (var address in retVal.Addresses)
            {
                //Reset primary key for addresses
                address.Key = null;
            }
            retVal.TaxDetails = cart.TaxDetails;

            return retVal;
        }
    }
}
