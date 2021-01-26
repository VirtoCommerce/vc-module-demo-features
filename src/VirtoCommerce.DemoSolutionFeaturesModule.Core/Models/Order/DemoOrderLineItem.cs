using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoOrderLineItem: LineItem
    {
        public string ConfiguredGroupId { get; set; }

        public CatalogProduct Product { get; set; }

        public override object Clone()
        {
            var result = base.Clone() as DemoCartLineItem;

            result.ConfiguredGroupId = ConfiguredGroupId;

            return result;
        }
    }
}
