using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoCartRepository : CartRepository
    {

        public IQueryable<DemoCartConfiguredGroupEntity> ConfiguredGroups => DbContext.Set<DemoCartConfiguredGroupEntity>();

        public IQueryable<DemoCartLineItemConfiguredGroupEntity> LineItmemConfiguredGroups => DbContext.Set<DemoCartLineItemConfiguredGroupEntity>();

        public DemoCartRepository(DemoCartDbContext dbContext)
            : base(dbContext)
        {
        }

        public override Task<ShoppingCartEntity[]> GetShoppingCartsByIdsAsync(string[] ids, string responseGroup = null)
        {
            var result = base.GetShoppingCartsByIdsAsync(ids, responseGroup);

            ConfiguredGroups.Where(x => ids.Contains(x.ShoppingCartId)).Load();

            var groupIds = ConfiguredGroups.Select(x => x.Id).ToArray();

            LineItmemConfiguredGroups.Where(x => groupIds.Contains(x.GroupId)).Load();

            return result;
        }
    }
}
