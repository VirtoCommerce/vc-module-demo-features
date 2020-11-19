using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Search;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoCustomerSegmentSearchService
    {
        public Task<DemoCustomerSegmentSearchResult> SearchCustomerSegmentsAsync(DemoCustomerSegmentSearchCriteria criteria);
    }
}
