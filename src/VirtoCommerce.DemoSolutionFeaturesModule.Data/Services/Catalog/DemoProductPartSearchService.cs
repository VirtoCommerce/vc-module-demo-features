using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Catalog
{
    public class DemoProductPartSearchService : IDemoProductPartSerarchService
    {
        private readonly Func<ICatalogRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IDemoProductPartService _productPartService;

        public DemoProductPartSearchService(
            Func<ICatalogRepository> catalogRepositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            IDemoProductPartService productPartService)
        {
            _repositoryFactory = catalogRepositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _productPartService = productPartService;
        }

        public virtual async Task<DemoProductPartSearchResult> SearchProductPartsAsync(DemoProductPartSearchCriteria criteria)
        {
            ValidateParameters(criteria);

            var cacheKey = CacheKey.With(GetType(), nameof(SearchProductPartsAsync), criteria.GetCacheKey());

            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                cacheEntry.AddExpirationToken(DemoProductPartSearchCacheRegion.CreateChangeToken());

                var result = AbstractTypeFactory<DemoProductPartSearchResult>.TryCreateInstance();

                using (var catalogRepository = _repositoryFactory())
                {
                    //Optimize performance and CPU usage
                    catalogRepository.DisableChangesTracking();
                    var sortInfos = BuildSortExpression(criteria);
                    var query = BuildQuery(catalogRepository, criteria);

                    result.TotalCount = await query.CountAsync();

                    if (criteria.Take > 0 && result.TotalCount > 0)
                    {
                        var ids = await query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id)
                            .Select(x => x.Id)
                            .Skip(criteria.Skip).Take(criteria.Take)
                            .AsNoTracking()
                            .ToArrayAsync();

                        result.Results = (await _productPartService.GetByIdsAsync(ids)).OrderBy(x => Array.IndexOf(ids, x.Id)).ToList();
                    }
                }

                return result;
            });
        }

        protected virtual IQueryable<DemoProductPartEntity> BuildQuery(ICatalogRepository catalogRepository, DemoProductPartSearchCriteria criteria)
        {
            var query = ((DemoCatalogRepository)catalogRepository).ConfiguredProductParts;

            if (!string.IsNullOrEmpty(criteria.Keyword))
            {
                query = query.Where(x => x.Name.Contains(criteria.Keyword));
            }

            if (criteria.ConfiguredProductId != null)
            {
                query = query.Where(x => x.ConfiguredProductId == criteria.ConfiguredProductId);
            }

            return query;
        }

        protected virtual IList<SortInfo> BuildSortExpression(DemoProductPartSearchCriteria criteria)
        {
            var sortInfos = criteria.SortInfos;

            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[] { new SortInfo { SortColumn = nameof(DemoProductPart.Name) } };
            }

            return sortInfos;
        }

        private static void ValidateParameters(DemoProductPartSearchCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
        }
    }
}
