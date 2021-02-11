using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CustomerModule.Data.Caching;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Customer
{
    public class DemoTaggedMemberService : IDemoTaggedMemberService
    {

        private readonly Func<IDemoTaggedMemberRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IEventPublisher _eventPublisher;

        public DemoTaggedMemberService(Func<IDemoTaggedMemberRepository> repositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            IEventPublisher eventPublisher)
        {
            _repositoryFactory = repositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _eventPublisher = eventPublisher;
        }

        public async Task DeleteAsync(string[] ids)
        {
            var taggedMembers = await GetByIdsAsync(ids);
            var changedEntries = taggedMembers
                .Select(x => new GenericChangedEntry<DemoTaggedMember>(x, EntryState.Deleted))
                .ToArray();

            using var repository = _repositoryFactory();

            await _eventPublisher.Publish(new DemoTaggedMemberChangingEvent(changedEntries));

            var taggedMemberEntities = await repository.GetTaggedMembersByIdsAsync(ids);

            foreach (var memberEntity in taggedMemberEntities)
            {
                repository.Remove(memberEntity);
            }

            await repository.UnitOfWork.CommitAsync();

            ClearCache(taggedMembers);

            await _eventPublisher.Publish(new DemoTaggedMemberChangedEvent(changedEntries));
        }

        public async Task<DemoTaggedMember[]> GetByIdsAsync(string[] ids)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", ids));

            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
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
            ValidateTaggedMembersArgument(taggedMembers);

            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<DemoTaggedMember>>();

            using var repository = _repositoryFactory();

            var ids = taggedMembers.Select(x => x.Id).Where(x => x != null).Distinct().ToArray();
            var alreadyExistEntities = await repository.GetTaggedMembersByIdsAsync(ids);
            foreach (var taggedMember in taggedMembers)
            {
                var sourceEntity = AbstractTypeFactory<DemoTaggedMemberEntity>.TryCreateInstance().FromModel(taggedMember, pkMap);
                var targetEntity = alreadyExistEntities.FirstOrDefault(x => x.Id == taggedMember.Id);

                if (targetEntity != null)
                {
                    sourceEntity.Patch(targetEntity);
                    changedEntries.Add(new GenericChangedEntry<DemoTaggedMember>(taggedMember, targetEntity.ToModel(AbstractTypeFactory<DemoTaggedMember>.TryCreateInstance()), EntryState.Modified));
                }
                else
                {
                    repository.Add(sourceEntity);
                    changedEntries.Add(new GenericChangedEntry<DemoTaggedMember>(taggedMember, EntryState.Added));
                }
            }

            await _eventPublisher.Publish(new DemoTaggedMemberChangingEvent(changedEntries));

            await repository.UnitOfWork.CommitAsync();
            pkMap.ResolvePrimaryKeys();

            ClearCache(taggedMembers);

            await _eventPublisher.Publish(new DemoTaggedMemberChangedEvent(changedEntries));
        }

        private static void ValidateTaggedMembersArgument(DemoTaggedMember[] taggedMembers)
        {
            if (taggedMembers.Any(x => x.Id.IsNullOrEmpty()))
            {
                throw new ArgumentNullException(nameof(taggedMembers),
                    "The argument should have no item with the 'Id' field is null.");
            }
        }


        protected virtual void ClearCache(DemoTaggedMember[] taggedMembers)
        {
            foreach (var taggedMember in taggedMembers)
            {
                DemoTaggedMemberCacheRegion.ExpireEntity(taggedMember);
                CustomerCacheRegion.ExpireMemberById(taggedMember.Id);
            }

            DemoTaggedMemberSearchCacheRegion.ExpireRegion();
        }
    }
}
