using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer
{
    public interface IDemoTaggedMemberRepository : IRepository
    {
        public IQueryable<DemoTaggedMemberEntity> TaggedMembers { get; }

        public IQueryable<DemoMemberTagEntity> MemberTags { get; }

        public Task<DemoTaggedMemberEntity[]> GetTaggedMembersByIdsAsync(string[] ids);
    }
}
