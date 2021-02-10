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
    public class TaggedMemberService : ITaggedMemberService
    {

        private readonly Func<ITaggedMemberRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;

        public TaggedMemberService(Func<ITaggedMemberRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache)
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

        public async Task<TaggedMember[]> GetByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", ids));

            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey,
                async (cacheEntry) =>
                {
                    cacheEntry.AddExpirationToken(TaggedMemberCacheRegion.CreateChangeToken());
                    using var repository = _repositoryFactory();
                    repository.DisableChangesTracking();
                    var memberEntities = await repository.GetTaggedMembersByIdsAsync(ids);
                    return memberEntities.Select(x => x.ToModel(AbstractTypeFactory<TaggedMember>.TryCreateInstance())).ToArray();
                });
            return result;
        }

        public async Task SaveChangesAsync(TaggedMember[] taggedMembers)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            {
                var ids = taggedMembers.Select(x => x.Id).Where(x => x != null).Distinct().ToArray();
                var alreadyExistEntities = await repository.GetTaggedMembersByIdsAsync(ids);
                foreach (var taggedMember in taggedMembers)
                {
                    var sourceEntity = AbstractTypeFactory<TaggedMemberEntity>.TryCreateInstance().FromModel(taggedMember, pkMap);
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


        protected virtual void ClearCache(TaggedMember[] taggedMembers)
        {
            foreach (var member in taggedMembers)
            {
                TaggedMemberCacheRegion.ExpireEntity(member);
                CustomerCacheRegion.ExpireMemberById(member.MemberId);
            }

            TaggedMemberSearchCacheRegion.ExpireRegion();
        }
    }
}
