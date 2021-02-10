using System.Collections.Generic;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Customer
{
    public class DemoTaggedMemberChangedEvent : GenericChangedEntryEvent<DemoTaggedMember>
    {
        public DemoTaggedMemberChangedEvent(IEnumerable<GenericChangedEntry<DemoTaggedMember>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
