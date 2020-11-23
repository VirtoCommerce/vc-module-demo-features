using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.CustomerSegment
{
    public class DemoCustomerSegmentConditionEvaluator: IDemoCustomerSegmentConditionEvaluator
    {
        private readonly IDemoCustomerSegmentSearchRequestBuilder _requestBuilder;
        private readonly ISearchProvider _searchProvider;

        public DemoCustomerSegmentConditionEvaluator(
            IDemoCustomerSegmentSearchRequestBuilder requestBuilder,
            ISearchProvider searchProvider
        )
        {
            _requestBuilder = requestBuilder;
            _searchProvider = searchProvider;
        }

        public async Task<string[]> EvaluateCustomerSegmentConditionAsync(DemoCustomerSegmentConditionEvaluationRequest conditionRequest)
        {
            _requestBuilder
                .AddPropertySearch(conditionRequest.Properties)
                .AddSortInfo(conditionRequest.SortInfos)
                .WithStores(conditionRequest.StoreIds)
                .WithPaging(conditionRequest.Skip, conditionRequest.Take);

            var searchResult = await _searchProvider.SearchAsync(KnownDocumentTypes.Member, _requestBuilder.Build());

            return searchResult.Documents.Select(x => x.Id).ToArray();
        }
    }
}
