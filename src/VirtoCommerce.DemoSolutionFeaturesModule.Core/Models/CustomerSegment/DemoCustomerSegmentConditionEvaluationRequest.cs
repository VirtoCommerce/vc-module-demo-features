using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCustomerSegmentConditionEvaluationRequest
    {
        public DemoCustomerSegmentConditionEvaluationRequest()
        {
            Properties = new Dictionary<string, string[]>();
            SortInfos = new List<SortInfo>();
        }

        public IDictionary<string, string[]> Properties { get; set; }

        public string Sort { get; set; }

        public IList<SortInfo> SortInfos { get; set; }

        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 20;
    }
}
