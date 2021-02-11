using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer
{
    public interface IDemoTaggedMemberService
    {
        Task<DemoTaggedMember[]> GetByIdsAsync(string[] memberIds);

        Task SaveChangesAsync(DemoTaggedMember[] taggedMembers);

        Task DeleteAsync(string[] memberIds);
    }
}
