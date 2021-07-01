using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Customer
{
    public class DemoTaggedMemberSearchService : IDemoTaggedMemberSearchService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IDemoTaggedMemberService _taggedMemberService;

        public DemoTaggedMemberSearchService(Func<ICustomerRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IDemoTaggedMemberService taggedMemberService)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _taggedMemberService = taggedMemberService;
        }

        public async Task<DemoTaggedMemberSearchResult> SearchTaggedMembersAsync(DemoTaggedMemberSearchCriteria criteria)
        {
            ValidateParameters(criteria);

            var cacheKey = CacheKey.With(GetType(), nameof(SearchTaggedMembersAsync), criteria.GetCacheKey());

            return await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey,
                async (cacheEntry) =>
                {
                    cacheEntry.AddExpirationToken(DemoTaggedMemberSearchCacheRegion.CreateChangeToken());
                    var result = new DemoTaggedMemberSearchResult();

                    using var taggedMemberRepository = (IDemoTaggedMemberRepository)_repositoryFactory();
                    taggedMemberRepository.DisableChangesTracking();

                    var query = taggedMemberRepository.TaggedMembers;

                    if (!criteria.MemberIds.IsNullOrEmpty())
                    {
                        query = query.Where(x => criteria.MemberIds.Contains(x.Id));
                    }

                    if (criteria.ChangedFrom.HasValue)
                    {
                        query = query.Where(x => x.ModifiedDate.HasValue && x.ModifiedDate.Value.Date >= criteria.ChangedFrom.Value.Date);
                    }

                    var sortInfos = criteria.SortInfos;
                    if (sortInfos.IsNullOrEmpty())
                    {
                        sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<DemoTaggedMember>(x => x.Id) } };
                    }

                    query = query.OrderBySortInfos(sortInfos).ThenBy(x => x.Id);

                    result.TotalCount = await query.CountAsync();
                    query = query.Skip(criteria.Skip).Take(criteria.Take);

                    if (criteria.Take > 0)
                    {
                        var ids = query.Select(x => x.Id).ToArray();
                        var selectedMembers = await _taggedMemberService.GetByIdsAsync(ids);
                        result.Results = selectedMembers.OrderBy(x => Array.IndexOf(ids, x.Id)).ToList();

                    }

                    return result;
                });
        }

        private static void ValidateParameters(DemoTaggedMemberSearchCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }
        }
    }
}
