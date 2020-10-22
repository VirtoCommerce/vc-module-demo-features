using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoCartRepository : CartRepository
    {
        public IQueryable<DemoCartConfiguredGroupEntity> ConfiguredGroups => DbContext.Set<DemoCartConfiguredGroupEntity>();

        public DemoCartRepository(DemoCartDbContext dbContext)
            : base(dbContext)
        {
        }

        public override Task<ShoppingCartEntity[]> GetShoppingCartsByIdsAsync(string[] ids, string responseGroup = null)
        {
            var result = base.GetShoppingCartsByIdsAsync(ids, responseGroup);

            ConfiguredGroups.Where(x => ids.Contains(x.ShoppingCartId)).Load();

            return result;
        }
    }
}
