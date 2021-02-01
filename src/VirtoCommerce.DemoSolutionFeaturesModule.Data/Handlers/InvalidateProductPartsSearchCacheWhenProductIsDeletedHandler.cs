using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Core.Events;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Catalog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Handlers
{
    public class InvalidateProductPartsSearchCacheWhenProductIsDeletedHandler : IEventHandler<ProductChangedEvent>
    {
        public Task Handle(ProductChangedEvent message)
        {
            if (message.ChangedEntries.Any(x => x.EntryState == EntryState.Deleted))
            {
                var changedEntries = message
                    .ChangedEntries
                    .Select(x => x.NewEntry)
                    .OfType<DemoProduct>()
                    .ToArray();

                if (changedEntries.All(x => !x.ProductParts.IsNullOrEmpty()))
                {
                    foreach (var demoProductPart in changedEntries.SelectMany(x => x.ProductParts))
                    {
                        DemoProductPartCacheRegion.ExpireEntity(demoProductPart);
                    }
                }
                else
                {
                    DemoProductPartCacheRegion.ExpireRegion();
                }
                
                DemoProductPartSearchCacheRegion.ExpireRegion();
            }

            return Task.CompletedTask;
        }
    }
}
