using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer
{
    public interface IDemoTaggedMemberSearchService
    {
        Task<DemoTaggedMemberSearchResult> SearchTaggedMembersAsync(DemoTaggedMemberSearchCriteria criteria);
    }
}
