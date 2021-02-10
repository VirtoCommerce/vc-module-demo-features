using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer
{
    public class DemoTaggedMemberSearchCriteria : SearchCriteriaBase
    {
        public string MemberId { get; set; }

        private string[] _memberIds;
        public string[] MemberIds
        {
            get
            {
                if (_memberIds == null && !string.IsNullOrEmpty(MemberId))
                {
                    _memberIds = new[] { MemberId };
                }
                return _memberIds;
            }
            set
            {
                _memberIds = value;
            }
        }

        public DateTime? ChangedFrom { get; set; }
        public string[] Ids { get; set; }
    }
}
