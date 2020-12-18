using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Catalog
{
    public class DemoProductPartService : IDemoProductPartService
    {
        private readonly Func<ICatalogRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IEventPublisher _eventPublisher;

        public DemoProductPartService(
            Func<ICatalogRepository> catalogRepositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            IEventPublisher eventPublisher)
        {
            _repositoryFactory = catalogRepositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _eventPublisher = eventPublisher;
        }

        public async Task<DemoProductPart[]> GetByIdsAsync(string[] partIds)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", partIds.OrderBy(x => x)));

            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                var parts = Array.Empty<DemoProductPart>();

                if (!partIds.IsNullOrEmpty())
                {
                    using var repository = (DemoCatalogRepository)_repositoryFactory();

                    //Optimize performance and CPU usage
                    repository.DisableChangesTracking();

                    var entities = await repository.GetProductPartsByIdsAsync(partIds);

                    parts = entities
                        .Select(x => x.ToModel(AbstractTypeFactory<DemoProductPart>.TryCreateInstance()))
                        .ToArray();

                    cacheEntry.AddExpirationToken(DemoProductPartCacheRegion.CreateChangeToken(partIds));
                }

                return parts;
            });

            return result;
        }

        public async Task SaveChangesAsync(DemoProductPart[] parts)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<DemoProductPart>>();

            using var repository = (DemoCatalogRepository)_repositoryFactory();

            var ids = parts.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray();
            var dbExistProducts = await repository.GetProductPartsByIdsAsync(ids);

            foreach (var part in parts)
            {
                var modifiedEntity = AbstractTypeFactory<DemoProductPartEntity>.TryCreateInstance().FromModel(part, pkMap);
                var originalEntity = dbExistProducts.FirstOrDefault(x => x.Id == part.Id);

                if (originalEntity != null)
                {
                    changedEntries.Add(new GenericChangedEntry<DemoProductPart>(part, originalEntity.ToModel(AbstractTypeFactory<DemoProductPart>.TryCreateInstance()), EntryState.Modified));
                    modifiedEntity.Patch(originalEntity);
                }
                else
                {
                    repository.Add(modifiedEntity);
                    changedEntries.Add(new GenericChangedEntry<DemoProductPart>(part, EntryState.Added));
                }
            }

            await _eventPublisher.Publish(new DemoProductPartChangingEvent(changedEntries));

            await repository.UnitOfWork.CommitAsync();
            pkMap.ResolvePrimaryKeys();

            ClearCache(parts);

            await _eventPublisher.Publish(new DemoProductPartChangedEvent(changedEntries));
        }

        public async Task DeleteAsync(string[] ids)
        {
            var parts = await GetByIdsAsync(ids);
            var changedEntries = parts
                .Select(x => new GenericChangedEntry<DemoProductPart>(x, EntryState.Deleted))
                .ToArray();

            using var repository = (DemoCatalogRepository)_repositoryFactory();

            await _eventPublisher.Publish(new DemoProductPartChangingEvent(changedEntries));

            var partEntities = await repository.GetProductPartsByIdsAsync(ids);

            foreach (var part in partEntities)
            {
                repository.Remove(part);
            }

            await repository.UnitOfWork.CommitAsync();

            ClearCache(parts);

            await _eventPublisher.Publish(new DemoProductPartChangedEvent(changedEntries));
        }

        protected virtual void ClearCache(IEnumerable<DemoProductPart> productParts)
        {
            foreach (var part in productParts)
            {
                DemoProductPartCacheRegion.ExpireEntity(part);
            }

            DemoProductPartSearchCacheRegion.ExpireRegion();
        }
    }
}
