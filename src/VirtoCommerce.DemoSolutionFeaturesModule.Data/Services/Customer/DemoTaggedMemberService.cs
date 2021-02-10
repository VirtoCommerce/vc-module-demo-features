using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CustomerModule.Data.Caching;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Customer
{
    public class DemoTaggedMemberService : IDemoTaggedMemberService
    {

        private readonly Func<IDemoTaggedMemberRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;

        public DemoTaggedMemberService(Func<IDemoTaggedMemberRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
        }

        public async Task DeleteAsync(string[] ids)
        {
            var taggedMembers = await GetByIdsAsync(ids);

            using var repository = _repositoryFactory();

            var taggedMemberEntities = await repository.GetTaggedMembersByIdsAsync(ids);

            foreach (var memberEntity in taggedMemberEntities)
            {
                repository.Remove(memberEntity);
            }

            await repository.UnitOfWork.CommitAsync();

            ClearCache(taggedMembers);
        }

        public async Task<DemoTaggedMember[]> GetByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", ids));

            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey,async (cacheEntry) =>
            {
                var taggedMembers = Array.Empty<DemoTaggedMember>();

                if (!ids.IsNullOrEmpty())
                {
                    using var repository = _repositoryFactory();
                    repository.DisableChangesTracking();
                    var memberEntities = await repository.GetTaggedMembersByIdsAsync(ids);
                    taggedMembers = memberEntities.Select(x => x.ToModel(AbstractTypeFactory<DemoTaggedMember>.TryCreateInstance())).ToArray();
                    cacheEntry.AddExpirationToken(DemoTaggedMemberCacheRegion.CreateChangeToken(taggedMembers));
                }
                
                return taggedMembers;
            });
            return result;
        }

        public async Task SaveChangesAsync(DemoTaggedMember[] taggedMembers)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                var ids = taggedMembers.Select(x => x.Id).Where(x => x != null).Distinct().ToArray();
                var alreadyExistEntities = await repository.GetTaggedMembersByIdsAsync(ids);
                foreach (var taggedMember in taggedMembers)
                {
                    var sourceEntity = AbstractTypeFactory<DemoTaggedMemberEntity>.TryCreateInstance().FromModel(taggedMember, pkMap);
                    var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.Id == taggedMember.Id);
                    if (targetEntity != null)
                    {
                        sourceEntity.Patch(targetEntity);
                    }
                    else
                    {
                        repository.Add(sourceEntity);
                    }
                }

                await repository.UnitOfWork.CommitAsync();
                pkMap.ResolvePrimaryKeys();
            }

            ClearCache(taggedMembers);
        }


        protected virtual void ClearCache(DemoTaggedMember[] taggedMembers)
        {
            foreach (var member in taggedMembers)
            {
                DemoTaggedMemberCacheRegion.ExpireEntity(member);
                CustomerCacheRegion.ExpireMemberById(member.MemberId);
            }

            DemoTaggedMemberSearchCacheRegion.ExpireRegion();
        }
    }
}
