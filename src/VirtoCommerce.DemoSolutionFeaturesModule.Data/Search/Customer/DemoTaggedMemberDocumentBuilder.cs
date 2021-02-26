using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.SearchModule.Core.Extenstions;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Search.Customer
{
    public class DemoTaggedMemberDocumentBuilder : IIndexDocumentBuilder
    {
        private readonly IDemoTaggedMemberService _taggedMemberService;

        public DemoTaggedMemberDocumentBuilder(IDemoTaggedMemberService taggedMemberService)
        {
            _taggedMemberService = taggedMemberService;
        }

        public async Task<IList<IndexDocument>> GetDocumentsAsync(IList<string> documentIds)
        {
            IList<IndexDocument> result = new List<IndexDocument>();

            var taggedMembers = await _taggedMemberService.GetByIdsAsync(documentIds.ToArray());

            foreach (var taggedMember in taggedMembers)
            {
                var allMemberTags = taggedMember.Tags.Union(taggedMember.InheritedTags).OrderBy(x => x).ToArray();
                var ancestorDocument = CreateDocument(taggedMember.MemberId, allMemberTags);
                result.Add(ancestorDocument);
            }

            return result;
        }

        protected virtual IndexDocument CreateDocument(string id, string[] tags)
        {
            var document = new IndexDocument(id);
            document.AddFilterableValues("Groups", tags);
            return document;
        }
    }
}
