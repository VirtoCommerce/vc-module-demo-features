using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer
{
    public class DemoTaggedMemberSearchCriteria : SearchCriteriaBase
    {
        public string[] MemberIds { get; set; }
        public DateTime? ChangedFrom { get; set; }
        public string[] Ids { get; set; }
    }
}
