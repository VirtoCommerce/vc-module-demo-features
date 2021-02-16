using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;

namespace VirtoCommerce.CatalogPersonalizationModule.Data.Search.Indexing
{
    public class DemoTaggedMemberIndexChangesProvider : IIndexDocumentChangesProvider
    {
        public const string ChangeLogObjectType = nameof(DemoTaggedMember);

        private readonly IDemoTaggedMemberSearchService _taggedItemSearchService;
        private readonly IChangeLogSearchService _changeLogSearchService;

        public DemoTaggedMemberIndexChangesProvider(IDemoTaggedMemberSearchService taggedItemSearchService, IChangeLogSearchService changeLogSearchService)
        {
            _taggedItemSearchService = taggedItemSearchService;
            _changeLogSearchService = changeLogSearchService;
        }

        public async Task<long> GetTotalChangesCountAsync(DateTime? startDate, DateTime? endDate)
        {
            long result;

            if (startDate == null && endDate == null)
            {
                // We don't know the total products count
                result = 0L;
            }
            else
            {
                // Get changes count from operation log
                var searchResult = await _changeLogSearchService.SearchAsync(new ChangeLogSearchCriteria
                {
                    ObjectType = ChangeLogObjectType,
                    StartDate = startDate,
                    EndDate = endDate
                });

                result = searchResult.TotalCount;
            }

            return result;
        }

        public async Task<IList<IndexDocumentChange>> GetChangesAsync(DateTime? startDate, DateTime? endDate, long skip, long take)
        {
            IList<IndexDocumentChange> result;

            if (startDate == null && endDate == null)
            {
                result = null;
            }
            else
            {
                var searchResult = await _changeLogSearchService.SearchAsync(new ChangeLogSearchCriteria
                {
                    ObjectType = ChangeLogObjectType,
                    StartDate = startDate,
                    EndDate = endDate,
                    Skip = (int)skip,
                    Take = (int)take

                });

                result = searchResult.Results
                    .Select(o => new IndexDocumentChange
                    {
                        DocumentId = o.ObjectId, // TaggedMember.Id equals Member.Id
                        ChangeDate = o.ModifiedDate ?? o.CreatedDate,
                        ChangeType = IndexDocumentChangeType.Modified,
                    })
                    .ToArray();
            }

            return result;
        }
    }
}
