using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer
{
    public class TaggedMember : AuditableEntity
    {
        public string MemberType { get; set; }

        public string MemberId { get; set; }

        public ICollection<string> Tags { get; set; }

        public ICollection<string> InheritedTags { get; set; }
    }
}
