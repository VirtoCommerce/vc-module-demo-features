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
        private readonly Func<IDemoTaggedMemberRepository> _taggedMemberRepositoryFactory;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDemoMemberInheritanceEvaluator _memberInheritanceEvaluator;

        public DemoTaggedMemberService(Func<IDemoTaggedMemberRepository> taggedMemberRepositoryFactory,
            IPlatformMemoryCache platformMemoryCache,
            IEventPublisher eventPublisher,
            IDemoMemberInheritanceEvaluator memberInheritanceEvaluator)
        {
            _taggedMemberRepositoryFactory = taggedMemberRepositoryFactory;
            _platformMemoryCache = platformMemoryCache;
            _eventPublisher = eventPublisher;
            _memberInheritanceEvaluator = memberInheritanceEvaluator;
        }

        public async Task DeleteAsync(string[] memberIds)
        {
            var taggedMembers = await GetByIdsAsync(memberIds);
            var changedEntries = taggedMembers
                .Select(x => new GenericChangedEntry<DemoTaggedMember>(x, EntryState.Deleted))
                .ToArray();

            using var taggedMemberRepository = _taggedMemberRepositoryFactory();

            await _eventPublisher.Publish(new DemoTaggedMemberChangingEvent(changedEntries));

            var taggedMemberEntities = await taggedMemberRepository.GetTaggedMembersByIdsAsync(memberIds);

            foreach (var memberEntity in taggedMemberEntities)
            {
                taggedMemberRepository.Remove(memberEntity);
            }

            await taggedMemberRepository.UnitOfWork.CommitAsync();

            ClearCache(taggedMembers);

            await _eventPublisher.Publish(new DemoTaggedMemberChangedEvent(changedEntries));
        }

        public async Task<DemoTaggedMember[]> GetByIdsAsync(string[] memberIds)
        {
            var cacheKey = CacheKey.With(GetType(), nameof(GetByIdsAsync), string.Join("-", memberIds));

            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var taggedMembers = new List<DemoTaggedMember>();

                if (!memberIds.IsNullOrEmpty())
                {
                    var idsForCacheToken = new List<string>();
                    idsForCacheToken.AddRange(memberIds);

                    taggedMembers.AddRange(await GetTaggedMembersByIdsWithoutInheritedAsync(memberIds));

                    foreach (var memberId in memberIds)
                    {
                        var allAncestorsIds = (await _memberInheritanceEvaluator.GetAllAncestorIdsForMemberAsync(memberId)).ToArray();
                        var ancestorsTaggedMembers = await GetTaggedMembersByIdsWithoutInheritedAsync(allAncestorsIds);
                        var allAncestorsTags = ancestorsTaggedMembers.SelectMany(x => x.Tags).Distinct().ToArray();
                        Array.Sort(allAncestorsTags);

                        var taggedMember = taggedMembers.FirstOrDefault(x => x.Id == memberId);

                        if (taggedMember != null)
                        {
                            taggedMember.InheritedTags = allAncestorsTags;
                        }
                        else if (!allAncestorsTags.IsNullOrEmpty())
                        {
                            taggedMember = AbstractTypeFactory<DemoTaggedMember>.TryCreateInstance();
                            taggedMember.Id = memberId;
                            taggedMember.Tags = Array.Empty<string>();
                            taggedMember.InheritedTags = allAncestorsTags;
                            taggedMembers.Add(taggedMember);
                        }

                        idsForCacheToken.AddRange(allAncestorsIds);
                    }

                    cacheEntry.AddExpirationToken(DemoTaggedMemberCacheRegion.CreateChangeToken(idsForCacheToken.Distinct().ToArray()));
                }

                return taggedMembers.ToArray();
            });
            return result;
        }

        protected virtual async Task<DemoTaggedMember[]> GetTaggedMembersByIdsWithoutInheritedAsync(string[] memberIds)
        {
            using var taggedMemberRepository = _taggedMemberRepositoryFactory();
            taggedMemberRepository.DisableChangesTracking();
            var taggedMemberEntities = await taggedMemberRepository.GetTaggedMembersByIdsAsync(memberIds);
            var taggedMembers = taggedMemberEntities.Select(x => x.ToModel(AbstractTypeFactory<DemoTaggedMember>.TryCreateInstance()))
                .ToArray();
            return taggedMembers;
        }

        public async Task SaveChangesAsync(DemoTaggedMember[] taggedMembers)
        {
            ValidateTaggedMembersArgument(taggedMembers);

            var pkMap = new PrimaryKeyResolvingMap();
            var changedEntries = new List<GenericChangedEntry<DemoTaggedMember>>();

            using var taggedMemberRepository = _taggedMemberRepositoryFactory();

            var ids = taggedMembers.Select(x => x.Id).Where(x => x != null).Distinct().ToArray();
            var alreadyExistEntities = await taggedMemberRepository.GetTaggedMembersByIdsAsync(ids);

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
                    taggedMemberRepository.Add(sourceEntity);
                    changedEntries.Add(new GenericChangedEntry<DemoTaggedMember>(taggedMember, EntryState.Added));
                }
            }

            await _eventPublisher.Publish(new DemoTaggedMemberChangingEvent(changedEntries));

            await taggedMemberRepository.UnitOfWork.CommitAsync();
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
