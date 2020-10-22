using System.Linq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Services;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services
{
    public class DemoShoppingCartTotalsCalculator : DefaultShoppingCartTotalsCalculator
    {
        public override void CalculateTotals(ShoppingCart cart)
        {
            base.CalculateTotals(cart);
            CalculateConfiguredGroups(cart);
        }

        private static void CalculateConfiguredGroups(ShoppingCart cart)
        {
            var cartExtended = (DemoShoppingCart)cart;

            var configuredGroups = (cartExtended.ConfiguredGroups ?? Enumerable.Empty<DemoCartConfiguredGroup>()).ToArray();

            foreach (var configuredGroup in configuredGroups)
            {
                var lineItems = configuredGroup.Items;
                // Quantity if line item in mixed tote = quantity of line item of single mixed tote * mixed tote quantity
                configuredGroup.ListPrice = lineItems.Sum(x => x.PlacedPrice * (x.Quantity / configuredGroup.Quantity));
                configuredGroup.ListPriceWithTax = lineItems.Sum(x => x.PlacedPriceWithTax * (x.Quantity / configuredGroup.Quantity));
                configuredGroup.SalePrice = configuredGroup.ListPrice;
                configuredGroup.SalePriceWithTax = configuredGroup.ListPriceWithTax;
                configuredGroup.PlacedPrice = configuredGroup.ListPrice;
                configuredGroup.PlacedPriceWithTax = configuredGroup.ListPriceWithTax;
                configuredGroup.ExtendedPrice = configuredGroup.PlacedPrice * configuredGroup.Quantity;
                configuredGroup.ExtendedPriceWithTax = configuredGroup.PlacedPriceWithTax * configuredGroup.Quantity;
                configuredGroup.ExtendedPrice = lineItems.Sum(x => x.ExtendedPrice);
                configuredGroup.TaxTotal = lineItems.Sum(x => x.TaxTotal);
                configuredGroup.ExtendedPriceWithTax = lineItems.Sum(x => x.ExtendedPriceWithTax);
            }
        }
    }
}
