using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer
{
    public interface ITaggedMemberService
    {
        Task<TaggedMember[]> GetByIdsAsync(string[] ids);
        Task SaveChangesAsync(TaggedMember[] taggedMembers);
        Task DeleteAsync(string[] ids);
    }
}
