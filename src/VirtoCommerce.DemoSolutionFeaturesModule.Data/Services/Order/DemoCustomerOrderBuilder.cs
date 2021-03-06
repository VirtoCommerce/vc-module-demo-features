using System.Linq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.OrdersModule.Data.Services;
using LineItem = VirtoCommerce.OrdersModule.Core.Model.LineItem;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services
{
    public class DemoCustomerOrderBuilder : CustomerOrderBuilder
    {
        public DemoCustomerOrderBuilder(ICustomerOrderService customerOrderService) : base(customerOrderService)
        {
        }

        protected override CustomerOrder ConvertCartToOrder(ShoppingCart cart)
        {
            var cartExtended = (DemoShoppingCart) cart;
            var orderExtended = (DemoCustomerOrder) base.ConvertCartToOrder(cart);

            orderExtended.Status = "Unpaid";

            orderExtended.ConfiguredGroups = cartExtended.ConfiguredGroups.Select(x => new DemoOrderConfiguredGroup
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                ItemIds = x.ItemIds,
                Items = orderExtended.Items?.Where(i => x.ItemIds.Contains(i.Id)).Select(i => (DemoOrderLineItem)i).ToArray(),
                Quantity = x.Quantity,
                Currency = x.Currency,
                Price = x.ListPrice,
                PriceWithTax = x.ListPriceWithTax,
                PlacedPrice = x.PlacedPrice,
                PlacedPriceWithTax = x.PlacedPriceWithTax,
                ExtendedPrice = x.ExtendedPrice,
                ExtendedPriceWithTax = x.ExtendedPriceWithTax,
                TaxTotal = x.TaxTotal
            }).ToList();

            return orderExtended;
        }

        protected override LineItem ToOrderModel(CartModule.Core.Model.LineItem lineItem)
        {
            var cartLineItem = (DemoCartLineItem)lineItem;
            var orderLineItem = (DemoOrderLineItem)base.ToOrderModel(lineItem);
            orderLineItem.Id = cartLineItem.Id;
            orderLineItem.ConfiguredGroupId = cartLineItem.ConfiguredGroupId;
            return orderLineItem;
        }
    }
}
