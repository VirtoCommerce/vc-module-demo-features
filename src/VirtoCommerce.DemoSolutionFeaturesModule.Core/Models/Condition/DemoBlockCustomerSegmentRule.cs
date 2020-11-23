using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoBlockCustomerSegmentRule: BlockConditionAndOr
    {
        public DemoBlockCustomerSegmentRule()
        {
            All = true;
        }

        public virtual IDictionary<string, string[]> GetPropertyValues()
        {
            return new Dictionary<string, string[]>();
        }
    }
}
