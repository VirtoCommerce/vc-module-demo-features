using VirtoCommerce.CartModule.Data.Repositories;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoCartRepository : CartRepository
    {
        public DemoCartRepository(DemoCartDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
