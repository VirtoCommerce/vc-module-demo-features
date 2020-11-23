using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.CustomerSegment
{
    public interface IDemoCustomerSegmentSearchRequestBuilder
    {
        SearchRequest Build();
        IDemoCustomerSegmentSearchRequestBuilder AddPropertySearch(IDictionary<string, string[]> propertyValues);
        IDemoCustomerSegmentSearchRequestBuilder AddSortInfo(IList<SortInfo> sortInfos);
        IDemoCustomerSegmentSearchRequestBuilder WithPaging(int skip, int take);
    }
}
