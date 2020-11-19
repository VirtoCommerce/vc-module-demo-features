using System.Collections.Generic;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoConditionPropertyValues: ConditionTree
    {
        public IDictionary<string, string[]> Properties { get; set; }

        public IDictionary<DynamicProperty, DynamicPropertyObjectValue> DynamicProperties { get; set; }

        public IDictionary<string, string[]> GetPropertyValues()
        {
            return new Dictionary<string, string[]>();
        }
    }
}
