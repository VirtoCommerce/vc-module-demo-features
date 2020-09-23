using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class CustomerDemoRepository : CustomerRepository
    {
        public CustomerDemoRepository(CustomerDemoDbContext dbContext) : base(dbContext)
        {           
        }

        public IQueryable<ContactDemoEntity> ContactsDemo => DbContext.Set<ContactDemoEntity>();       

    }
}
