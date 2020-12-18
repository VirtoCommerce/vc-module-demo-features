using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoProductPartSerarchService
    {
        Task<DemoProductPartSearchResult> SearchProductPartsAsync(DemoProductPartSearchCriteria criteria);
    }
}
