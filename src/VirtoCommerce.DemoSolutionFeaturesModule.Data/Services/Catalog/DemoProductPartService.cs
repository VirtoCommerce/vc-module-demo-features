using System;
using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Catalog
{
    public class DemoProductPartService : IDemoProductPartService
    {
        public Task DeleteAsync(string[] partIds)
        {
            throw new NotImplementedException();
        }

        public Task<DemoProductPart> GetByIdAsync(string id, string responseGroup, string catalogId = null)
        {
            throw new NotImplementedException();
        }

        public Task<DemoProductPart[]> GetByIdsAsync(string[] partIds, string respGroup, string catalogId = null)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync(DemoProductPart[] parts)
        {
            throw new NotImplementedException();
        }
    }
}
