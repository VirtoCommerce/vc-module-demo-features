using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoCustomerSegmentSearchRequestBuilder
    {
        SearchRequest Build();
        IDemoCustomerSegmentSearchRequestBuilder AddKeywordSearch(string keyword);
        IDemoCustomerSegmentSearchRequestBuilder AddPropertySearch(IDictionary<string, string[]> properties);
        IDemoCustomerSegmentSearchRequestBuilder WithStores(IList<string> storeIds);
        IDemoCustomerSegmentSearchRequestBuilder AddSortInfo(IList<SortInfo> sortInfos);
        IDemoCustomerSegmentSearchRequestBuilder WithPaging(int skip, int take);
    }
}
