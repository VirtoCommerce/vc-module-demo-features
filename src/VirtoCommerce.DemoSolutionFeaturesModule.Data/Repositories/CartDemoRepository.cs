using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.CartModule.Data.Repositories;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class CartDemoRepository : CartRepository
    {
        public CartDemoRepository(CartDemoDbContext dbContext) : base(dbContext)
        {
        }
    }
}
