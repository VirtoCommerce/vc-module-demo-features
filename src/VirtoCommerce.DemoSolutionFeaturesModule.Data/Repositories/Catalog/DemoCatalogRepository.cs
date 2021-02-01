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

        public override async Task RemoveItemsAsync(string[] itemIds)
        {
            if (!itemIds.IsNullOrEmpty())
            {
                var skip = 0;
                var batchSize = 500;
                do
                {
                    const string commandTemplate = @"
                        DELETE DPI FROM DemoProductPartItem DPI INNER JOIN Item I ON I.Id = DPI.ItemId
                        WHERE I.Id IN ({0})

                        DELETE DP FROM DemoProductPart DP INNER JOIN Item I ON I.Id = DP.ConfiguredProductId
                        WHERE I.Id IN ({0})
                    ";

                    await ExecuteStoreQueryAsync(commandTemplate, itemIds.Skip(skip).Take(batchSize));

                    skip += batchSize;

                } while (skip < itemIds.Length);
            }

            await base.RemoveItemsAsync(itemIds);
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
