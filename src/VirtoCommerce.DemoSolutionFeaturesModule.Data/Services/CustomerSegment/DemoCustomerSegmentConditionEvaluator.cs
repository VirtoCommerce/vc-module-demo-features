using System;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.CustomerSegment
{
    public class DemoCustomerSegmentConditionEvaluator: IDemoCustomerSegmentConditionEvaluator
    {
        public Task<string[]> EvaluateCustomerSegmentConditionAsync(DemoCustomerSegmentConditionEvaluationRequest conditionRequest)
        {
            return Task.FromResult(Array.Empty<string>());
        }
    }
}
