using System.Threading.Tasks;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer
{
    public interface IDemoMemberInheritanceEvaluator
    {
        Task<string[]> GetAllAncestorIdsForMemberAsync(string memberId);

        Task<string[]> GetAllDescendantIdsForMemberAsync(string memberId);
    }
}
