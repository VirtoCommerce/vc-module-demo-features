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
        public CustomerDemoRepository(CustomerDbContext dbContext) : base(dbContext)
        {           
        }

        public IQueryable<ContactDemoEntity> ContactsDemo => DbContext.Set<ContactDemoEntity>();

        public override async Task<MemberEntity[]> GetMembersByIdsAsync(string[] ids, string responseGroup = null, string[] memberTypes = null)
        {
            var retVal = await base.GetMembersByIdsAsync(ids, responseGroup, memberTypes);
            return retVal;
        }

    }
}
