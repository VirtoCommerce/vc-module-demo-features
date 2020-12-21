using System.Collections.Generic;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Catalog
{
    public class DemoProductPartChangedEvent : GenericChangedEntryEvent<DemoProductPart>
    {
        public DemoProductPartChangedEvent(IEnumerable<GenericChangedEntry<DemoProductPart>> changedEntries)
            : base(changedEntries)
        {
        }
    }
}
