using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoCatalogRepository : CatalogRepositoryImpl
    {
        public IQueryable<DemoProductPartEntity> ConfiguredProductParts => DbContext.Set<DemoProductPartEntity>();

        public DemoCatalogRepository(DemoCatalogDbContext dbContext)
            : base(dbContext)
        {
        }

        public virtual async Task<DemoProductPartEntity[]> GetProductPartsByIdsAsync(string[] ids)
        {
            var result = Array.Empty<DemoProductPartEntity>();

            if (!ids.IsNullOrEmpty())
            {
                result = await ConfiguredProductParts.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            }

            return result;
        }
    }
}
