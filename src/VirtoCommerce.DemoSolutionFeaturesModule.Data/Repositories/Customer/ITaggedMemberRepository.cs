using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer
{
    public interface ITaggedMemberRepository : IRepository
    {
        public IQueryable<TaggedMemberEntity> TaggedMembers { get; }

        public IQueryable<MemberTagEntity> MemberTags { get; }

        public Task<TaggedMemberEntity[]> GetTaggedMembersByIdsAsync(string[] ids);
    }
}
