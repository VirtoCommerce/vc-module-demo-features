using System.Threading.Tasks;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer
{
    public interface IDemoMemberInheritanceEvaluator
    {
        Task<string[]> GetAllAncestorIdsForMemberAsync(string memberId, int callCounter = 0);

        Task<string[]> GetAllDescendantIdsForMemberAsync(string memberId, int callCounter = 0);
    }
}
