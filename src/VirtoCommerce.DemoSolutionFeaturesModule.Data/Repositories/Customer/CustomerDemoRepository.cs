using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class CustomerDemoRepository : CustomerRepository, IDemoTaggedMemberRepository
    {
        public CustomerDemoRepository(CustomerDemoDbContext dbContext)
            : base(dbContext)
        {           
        }

        public IQueryable<ContactDemoEntity> ContactsDemo => DbContext.Set<ContactDemoEntity>();

        public IQueryable<DemoTaggedMemberEntity> TaggedMembers => DbContext.Set<DemoTaggedMemberEntity>().Include(x => x.Tags);

        public IQueryable<DemoMemberTagEntity> MemberTags => DbContext.Set<DemoMemberTagEntity>();

        public async Task<DemoTaggedMemberEntity[]> GetTaggedMembersByIdsAsync(string[] ids)
        {
            var result = Array.Empty<DemoTaggedMemberEntity>();
            if (!ids.IsNullOrEmpty())
            {
                result = await TaggedMembers.Where(x => ids.Contains(x.Id)).ToArrayAsync();
            }
            return result;
        }

    }
}
