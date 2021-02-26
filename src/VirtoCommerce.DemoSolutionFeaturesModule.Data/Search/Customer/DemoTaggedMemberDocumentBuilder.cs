using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Core.Extenstions;
using VirtoCommerce.SearchModule.Core.Model;
using VirtoCommerce.SearchModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Search.Customer
{
    public class DemoTaggedMemberDocumentBuilder : IIndexDocumentBuilder
    {
        private readonly IDemoMemberInheritanceEvaluator _memberInheritanceEvaluator;
        private readonly IDemoTaggedMemberService _taggedMemberService;

        public DemoTaggedMemberDocumentBuilder(IDemoTaggedMemberService taggedMemberService, IDemoMemberInheritanceEvaluator memberInheritanceEvaluator)
        {
            _memberInheritanceEvaluator = memberInheritanceEvaluator;
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

                var descendantIds = await _memberInheritanceEvaluator.GetAllDescendantIdsForMemberAsync(taggedMember.MemberId);

                if (!descendantIds.IsNullOrEmpty())
                {
                    var descendants = await _taggedMemberService.GetByIdsAsync(descendantIds);

                    foreach (var descendant in descendants)
                    {
                        var allDescendantTags = descendant.Tags.Union(descendant.InheritedTags).OrderBy(x => x).ToArray();

                        var descendantDocument = CreateDocument(descendant.MemberId, allDescendantTags);
                        result.Add(descendantDocument);
                    }
                }


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
