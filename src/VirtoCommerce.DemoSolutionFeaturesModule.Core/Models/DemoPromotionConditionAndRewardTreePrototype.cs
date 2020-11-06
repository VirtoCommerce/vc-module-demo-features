using VirtoCommerce.MarketingModule.Core.Model.Promotions;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public sealed class DemoPromotionConditionAndRewardTreePrototype: PromotionConditionAndRewardTreePrototype 
    {

        public DemoPromotionConditionAndRewardTreePrototype()
        {
            var blockCustomerSegments = new DemoBlockCustomerSegmentCondition().WithAvailConditions(new DemoCustomerSegmentCondition());
            AvailableChildren.Insert(0, blockCustomerSegments);
            Children.Insert(0, blockCustomerSegments);
        }
    }
}
