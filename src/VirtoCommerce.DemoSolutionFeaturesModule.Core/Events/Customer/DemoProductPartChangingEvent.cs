using System.Collections.Generic;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Customer
{
    public class DemoTaggedMemberChangingEvent : GenericChangedEntryEvent<DemoTaggedMember>
    {
        public DemoTaggedMemberChangingEvent(IEnumerable<GenericChangedEntry<DemoTaggedMember>> changingEntries)
            : base(changingEntries)
        {
        }
    }
}
