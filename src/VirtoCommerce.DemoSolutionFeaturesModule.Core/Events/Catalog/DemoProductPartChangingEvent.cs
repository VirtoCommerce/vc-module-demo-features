using System.Collections.Generic;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Catalog
{
    public class DemoProductPartChangingEvent : GenericChangedEntryEvent<DemoProductPart>
    {
        public DemoProductPartChangingEvent(IEnumerable<GenericChangedEntry<DemoProductPart>> changingEntries)
            : base(changingEntries)
        {
        }
    }
}
