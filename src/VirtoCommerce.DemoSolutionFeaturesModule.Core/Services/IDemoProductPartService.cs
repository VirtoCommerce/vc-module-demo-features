using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoProductPartService
    {
        Task<DemoProductPart[]> GetByIdsAsync(string[] partIds);        

        Task SaveChangesAsync(DemoProductPart[] parts);

        Task DeleteAsync(string[] partIds);
    }
}
