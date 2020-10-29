using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;
using VirtoCommerce.OrdersModule.Data.Model;
using VirtoCommerce.OrdersModule.Data.Repositories;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoOrderRepository: OrderRepository
    {
        public IQueryable<DemoOrderConfiguredGroupEntity> ConfiguredGroups => DbContext.Set<DemoOrderConfiguredGroupEntity>();

        public DemoOrderRepository(DemoOrderDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<CustomerOrderEntity[]> GetCustomerOrdersByIdsAsync(string[] ids, string responseGroup = null)
        {
            var result = await base.GetCustomerOrdersByIdsAsync(ids, responseGroup);

            ConfiguredGroups.Where(x => ids.Contains(x.CustomerOrderId)).Load();

            return result;
        }
    }
}
