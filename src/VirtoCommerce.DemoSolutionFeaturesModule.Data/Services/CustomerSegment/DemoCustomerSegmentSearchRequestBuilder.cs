using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.CustomerSegment
{
    public class DemoCustomerSegmentSearchRequestBuilder : IDemoCustomerSegmentSearchRequestBuilder
    {
        private readonly SearchRequest _searchRequest;

        public DemoCustomerSegmentSearchRequestBuilder()
        {
            _searchRequest = AbstractTypeFactory<SearchRequest>.TryCreateInstance();

            _searchRequest.Filter = new AndFilter { ChildFilters = new List<IFilter>(), };
            _searchRequest.Sorting = new List<SortingField> { new SortingField("__sort") };
            _searchRequest.Skip = 0;
            _searchRequest.Take = 20;
        }

        public virtual SearchRequest Build()
        {
            return _searchRequest;
        }

        public virtual IDemoCustomerSegmentSearchRequestBuilder AddPropertySearch(IDictionary<string, string[]> propertyValues)
        {
            if (!propertyValues.IsNullOrEmpty())
            {
                foreach (var propertyValue in propertyValues)
                {
                    ((AndFilter)_searchRequest.Filter).ChildFilters.Add(new TermFilter
                    {
                        FieldName = propertyValue.Key,
                        Values = propertyValue.Value
                    });
                }
            }

            return this;
        }

        public virtual IDemoCustomerSegmentSearchRequestBuilder AddSortInfo(IList<SortInfo> sortInfos)
        {
            if (!sortInfos.IsNullOrEmpty())
            {
                _searchRequest.Sorting = sortInfos.Select(x => new SortingField(x.SortColumn, x.SortDirection == SortDirection.Descending)).ToList();
            }

            return this;
        }

        public virtual IDemoCustomerSegmentSearchRequestBuilder WithPaging(int skip, int take)
        {
            _searchRequest.Skip = skip;
            _searchRequest.Take = take;

            return this;
        }
    }
}
