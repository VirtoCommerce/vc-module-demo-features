using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer
{
    public interface IDemoTaggedMemberRepository : IRepository
    {
        IQueryable<DemoTaggedMemberEntity> TaggedMembers { get; }

        IQueryable<DemoMemberTagEntity> MemberTags { get; }

        Task<DemoTaggedMemberEntity[]> GetTaggedMembersByIdsAsync(string[] ids);
    }
}
