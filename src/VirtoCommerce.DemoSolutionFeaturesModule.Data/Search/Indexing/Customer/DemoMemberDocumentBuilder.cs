using System.Linq;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.CustomerModule.Data.Search.Indexing;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.SearchModule.Core.Extenstions;
using VirtoCommerce.SearchModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Search.Indexing
{
    public class DemoMemberDocumentBuilder: MemberDocumentBuilder 
    {
        public DemoMemberDocumentBuilder(IMemberService memberService) : base(memberService)
        {
        }

        protected override IndexDocument CreateDocument(Member member)
        {
            var document = base.CreateDocument(member);
            if (member is IHasSecurityAccounts hasSecurityAccounts)
            {
                document.AddFilterableAndSearchableValues("Stores", hasSecurityAccounts.SecurityAccounts.Select(x => x.StoreId).ToArray());
            }
            return document;
        }
    }
}
