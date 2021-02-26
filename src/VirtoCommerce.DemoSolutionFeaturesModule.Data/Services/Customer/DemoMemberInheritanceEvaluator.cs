using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Customer
{
    public class DemoMemberInheritanceEvaluator : IDemoMemberInheritanceEvaluator
    {
        private readonly Func<IMemberRepository> _memberRepositoryFactory;

        public DemoMemberInheritanceEvaluator(Func<IMemberRepository> memberRepositoryFactory)
        {
            _memberRepositoryFactory = memberRepositoryFactory;
        }

        public virtual async Task<string[]> GetAllAncestorIdsForMemberAsync(string memberId, int callCounter = 0)
        {
            callCounter++;

            if (callCounter > Core.ModuleConstants.MaxRecursionDeep)
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
                    var ancestorsOfAncestorIds = await GetAllAncestorIdsForMemberAsync(ancestorId, callCounter);
                    result.AddRange(ancestorsOfAncestorIds);
                }
            }


            return result.Distinct().ToArray();
        }

        public virtual async Task<string[]> GetAllDescendantIdsForMemberAsync(string memberId, int callCounter = 0)
        {
            callCounter++;

            if (callCounter > Core.ModuleConstants.MaxRecursionDeep)
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
                    var descendantsOfDescendantIds = await GetAllDescendantIdsForMemberAsync(ancestorId, callCounter);
                    result.AddRange(descendantsOfDescendantIds);
                }
            }

            return result.Distinct().ToArray();
        }
    }
}
