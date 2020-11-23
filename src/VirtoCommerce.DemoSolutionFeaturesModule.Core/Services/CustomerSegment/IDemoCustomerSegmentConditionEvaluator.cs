using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoCustomerSegmentConditionEvaluator
    {
        Task<string[]> EvaluateCustomerSegmentConditionAsync(DemoCustomerSegmentConditionEvaluationRequest conditionRequest);
    }
}
