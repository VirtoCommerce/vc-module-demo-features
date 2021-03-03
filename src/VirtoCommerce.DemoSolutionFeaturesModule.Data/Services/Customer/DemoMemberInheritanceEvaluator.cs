using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Customer
{
    public class DemoMemberInheritanceEvaluator : IDemoMemberInheritanceEvaluator
    {
        private readonly Func<IMemberRepository> _memberRepositoryFactory;
        private readonly int _maxRecursionDeep;

        public DemoMemberInheritanceEvaluator(Func<IMemberRepository> memberRepositoryFactory, ISettingsManager settingsManager)
        {
            _memberRepositoryFactory = memberRepositoryFactory;
            _maxRecursionDeep = settingsManager.GetValue(ModuleConstants.Settings.General.DemoMaxRecursionDeep.Name,
                (int)ModuleConstants.Settings.General.DemoMaxRecursionDeep.DefaultValue);
        }

        public virtual async Task<string[]> GetAllAncestorIdsForMemberAsync(string memberId)
        {
            var callCounter = 0;

            async Task<string[]> InnerGetAllAncestorIdsForMemberAsync(string memberId)
            {
                callCounter++;

                if (callCounter > _maxRecursionDeep)
                {
                    return Array.Empty<string>();
                }

                var result = new List<string>();
                using var customerRepository = _memberRepositoryFactory();
                var relationEntities = await customerRepository.MemberRelations.Where(x =>
                        x.DescendantId == memberId)
                    .ToArrayAsync();

                var ancestorIds = relationEntities
                    .Where(x => x.RelationType.EqualsInvariant(RelationType.Membership.ToString()))
                    .Select(x => x.AncestorId).ToArray();

                if (!ancestorIds.IsNullOrEmpty())
                {
                    result.AddRange(ancestorIds);

                    foreach (var ancestorId in ancestorIds)
                    {
                        var ancestorsOfAncestorIds = await InnerGetAllAncestorIdsForMemberAsync(ancestorId);
                        result.AddRange(ancestorsOfAncestorIds);
                    }
                }


                return result.Distinct().ToArray();
            }

            return await InnerGetAllAncestorIdsForMemberAsync(memberId);
        }

        public virtual async Task<string[]> GetAllDescendantIdsForMemberAsync(string memberId)
        {
            var callCounter = 0;

            async Task<string[]> InnerGetAllDescendantIdsForMemberAsync(string memberId)
            {
                callCounter++;

                if (callCounter > _maxRecursionDeep)
                {
                    return Array.Empty<string>();
                }

                var result = new List<string>();
                using var customerRepository = _memberRepositoryFactory();

                var relationEntities = await customerRepository.MemberRelations.Where(x =>
                        x.AncestorId == memberId)
                    .ToArrayAsync();

                var descendantIds = relationEntities
                    .Where(x => x.RelationType.EqualsInvariant(RelationType.Membership.ToString()))
                    .Select(x => x.DescendantId).ToArray();

                if (!descendantIds.IsNullOrEmpty())
                {
                    result.AddRange(descendantIds);

                    foreach (var ancestorId in descendantIds)
                    {
                        var descendantsOfDescendantIds = await InnerGetAllDescendantIdsForMemberAsync(ancestorId);
                        result.AddRange(descendantsOfDescendantIds);
                    }
                }

                return result.Distinct().ToArray();
            }

            return await InnerGetAllDescendantIdsForMemberAsync(memberId);
        }
    }
}
