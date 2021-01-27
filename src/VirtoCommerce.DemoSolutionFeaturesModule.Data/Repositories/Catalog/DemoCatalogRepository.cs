using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.CatalogModule.Data.Repositories;
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

        public override async Task<ItemEntity[]> GetItemByIdsAsync(string[] itemIds, string responseGroup = null)
        {
            var result = await base.GetItemByIdsAsync(itemIds, responseGroup);
            var itemResponseGroup = EnumUtility.SafeParseFlags(responseGroup, ItemResponseGroup.ItemLarge);
            if (!itemIds.IsNullOrEmpty() && result.Any() && itemResponseGroup.HasFlag(ItemResponseGroup.ItemLarge))
            {
                await ConfiguredProductParts.Include(x => x.PartItems).Where(x => itemIds.Contains(x.ConfiguredProductId)).LoadAsync();
            }
            return result;
        }

        public virtual async Task<DemoProductPartEntity[]> GetProductPartsByIdsAsync(string[] ids)
        {
            var result = Array.Empty<DemoProductPartEntity>();

            if (!ids.IsNullOrEmpty())
            {
                result = await ConfiguredProductParts.Include(x => x.PartItems).Where(x => ids.Contains(x.Id)).ToArrayAsync();
            }

            return result;
        }
    }
}
