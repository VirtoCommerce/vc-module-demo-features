using System.Linq;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Data.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services
{
    public class DemoCustomerOrderTotalsCalculator : DefaultCustomerOrderTotalsCalculator
    {
        public override void CalculateTotals(CustomerOrder order)
        {
            base.CalculateTotals(order);
            CalculateConfiguredGroups(order);
        }

        private static void CalculateConfiguredGroups(CustomerOrder order)
        {
            var orderExtended = (DemoCustomerOrder) order;

            var configuredGroups = (orderExtended.ConfiguredGroups ?? Enumerable.Empty<DemoOrderConfiguredGroup>()).ToArray();

            foreach (var configuredGroup in configuredGroups)
            {
                var lineItems = orderExtended.Items.Where(x => configuredGroup.ItemIds.Contains(x.Id)).ToList();
                // Quantity if line item in mixed tote = quantity of line item of single mixed tote * mixed tote quantity
                configuredGroup.Price = lineItems.Sum(x => x.PlacedPrice * (x.Quantity / configuredGroup.Quantity));
                configuredGroup.PriceWithTax = lineItems.Sum(x => x.PlacedPriceWithTax * (x.Quantity / configuredGroup.Quantity));
                configuredGroup.PlacedPrice = configuredGroup.Price;
                configuredGroup.PlacedPriceWithTax = configuredGroup.PriceWithTax;
                configuredGroup.ExtendedPrice = configuredGroup.PlacedPrice * configuredGroup.Quantity;
                configuredGroup.ExtendedPriceWithTax = configuredGroup.PlacedPriceWithTax * configuredGroup.Quantity;
                configuredGroup.TaxTotal = lineItems.Sum(x => x.TaxTotal);
            }
        }
    }
}
