using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCustomerSegmentConditionEvaluationRequest
    {
        public IDictionary<string, string[]> Properties { get; set; }

        public string Sort { get; set; }

        public IList<SortInfo> SortInfos { get; set; }

        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 20;
    }
}
