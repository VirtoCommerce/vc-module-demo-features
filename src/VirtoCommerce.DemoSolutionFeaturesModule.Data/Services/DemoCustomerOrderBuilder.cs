using System.Linq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.OrdersModule.Data.Services;

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
            var orderExtended = (DemoCustomerOrder)base.ConvertCartToOrder(cart);

            orderExtended.Status = "Unpaid";

            orderExtended.ConfiguredGroups = cartExtended.ConfiguredGroups.Select(x => new DemoOrderConfiguredGroup
            {
                ProductId = x.ProductId,
                ItemIds = x.ItemIds,
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
    }
}
