using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.FeatureManagement;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security.Search;
using demoFeaturesCore = VirtoCommerce.DemoSolutionFeaturesModule.Core;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Customer
{
    public class DemoMemberService : MemberService
    {
        private readonly IDemoTaggedMemberService _taggedMemberService;
        private readonly IFeatureManager _featureManager;
        public DemoMemberService(Func<IMemberRepository> repositoryFactory, IUserSearchService userSearchService, IEventPublisher eventPublisher, IPlatformMemoryCache platformMemoryCache, IDemoTaggedMemberService taggedMemberService, IFeatureManager featureManager)
            : base(repositoryFactory, userSearchService, eventPublisher, platformMemoryCache)
        {
            _taggedMemberService = taggedMemberService;
            _featureManager = featureManager;
        }

        public override async Task<Member[]> GetByIdsAsync(string[] memberIds, string responseGroup = null, string[] memberTypes = null)
        {
            var members = await base.GetByIdsAsync(memberIds, responseGroup, memberTypes);

            var userGroupsInheritanceFeatureIsEnabled = await _featureManager.IsEnabledAsync(demoFeaturesCore.ModuleConstants.Features.UserGroupsInheritance);

            if (!members.IsNullOrEmpty() && userGroupsInheritanceFeatureIsEnabled)
            {
                var taggedMembers = await _taggedMemberService.GetByIdsAsync(memberIds);

                foreach (var member in members.Where(x => taggedMembers.Select(tm => tm.MemberId).Contains(x.Id)))
                {
                    var taggedMember = taggedMembers.First(x => x.MemberId == member.Id);
                    var tags = taggedMember.Tags.Union(taggedMember.InheritedTags ?? Array.Empty<string>()).ToList();
                    member.Groups = tags.IsNullOrEmpty() ? member.Groups : tags;
                }
            }

            return members;
        }

        public override async Task SaveChangesAsync(Member[] members)
        {
            var userGroupsInheritanceFeatureIsEnabled = await _featureManager.IsEnabledAsync(demoFeaturesCore.ModuleConstants.Features.UserGroupsInheritance);

            if (userGroupsInheritanceFeatureIsEnabled)
            {
                foreach (var member in members)
                {
                    member.Groups = new List<string>();
                }
            }

            await base.SaveChangesAsync(members);
        }
    }
}
