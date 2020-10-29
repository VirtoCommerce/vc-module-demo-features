using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.OrdersModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCustomerOrder: CustomerOrder
    {
        public ICollection<DemoOrderConfiguredGroup> ConfiguredGroups { get; set; }

        public override object Clone()
        {
            var result = base.Clone() as DemoCustomerOrder;

            result.ConfiguredGroups = ConfiguredGroups?.Select(x => x.Clone()).OfType<DemoOrderConfiguredGroup>().ToList();

            return result;
        }
    }
}