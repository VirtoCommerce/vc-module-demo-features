using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCustomerSegmentCondition: ConditionTree
    {
        public ICollection<string> MemberIds { get; set; }

        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            var result = false;
            if (context is PromotionEvaluationContext promotionEvaluationContext)
            {
                if (MemberIds != null)
                {
                    result = MemberIds.Any(x => x.EqualsInvariant(promotionEvaluationContext.CustomerId));
                }
            }

            return result;
        }
    }
}
