using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CartModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoShoppingCart : ShoppingCart
    {
        public ICollection<DemoCartConfiguredGroup> ConfiguredGroups { get; set; }

        public override object Clone()
        {
            var result = base.Clone() as DemoShoppingCart;

            result.ConfiguredGroups = ConfiguredGroups?.Select(x => x.Clone()).OfType<DemoCartConfiguredGroup>().ToList();

            return result;
        }
    }
}
