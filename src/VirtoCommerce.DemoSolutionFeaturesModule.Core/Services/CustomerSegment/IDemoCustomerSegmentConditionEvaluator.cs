using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoCustomerSegmentConditionEvaluator
    {
        public Task<string[]> EvaluateCustomerSegmentConditionAsync(DemoCustomerSegmentConditionEvaluationRequest conditionRequest);
    }
}
