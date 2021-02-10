using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer
{
    public interface IDemoTaggedMemberService
    {
        Task<DemoTaggedMember[]> GetByIdsAsync(string[] ids);
        Task SaveChangesAsync(DemoTaggedMember[] taggedMembers);
        Task DeleteAsync(string[] ids);
    }
}
