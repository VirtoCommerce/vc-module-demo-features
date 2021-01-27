using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCustomerOrder: CustomerOrder
    {

        public ICollection<LineItem> UsualItems => Items?.Where(x => ConfiguredGroups.IsNullOrEmpty() || !ConfiguredGroups.Any(y => y.Items.Contains(x))).ToArray();

        public ICollection<DemoOrderConfiguredGroup> ConfiguredGroups { get; set; }

        public override object Clone()
        {
            var result = base.Clone() as DemoCustomerOrder;

            result.ConfiguredGroups = ConfiguredGroups?.Select(x => x.Clone()).OfType<DemoOrderConfiguredGroup>().ToList();

            return result;
        }
    }
}
