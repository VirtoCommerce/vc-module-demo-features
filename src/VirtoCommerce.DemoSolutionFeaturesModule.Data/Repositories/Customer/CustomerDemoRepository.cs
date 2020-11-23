using System.Linq;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class CustomerDemoRepository : CustomerRepository
    {
        public CustomerDemoRepository(CustomerDemoDbContext dbContext)
            : base(dbContext)
        {           
        }

        public IQueryable<ContactDemoEntity> ContactsDemo => DbContext.Set<ContactDemoEntity>();       

    }
}
