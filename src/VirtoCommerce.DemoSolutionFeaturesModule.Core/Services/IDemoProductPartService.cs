using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoProductPartService
    {
        Task<DemoProductPart[]> GetByIdsAsync(string[] partIds, string respGroup, string catalogId = null);

        Task<DemoProductPart> GetByIdAsync(string id, string responseGroup, string catalogId = null);

        Task SaveChangesAsync(DemoProductPart[] parts);

        Task DeleteAsync(string[] partIds);
    }
}
