using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DesignTimeCatalogDbContextFactory : IDesignTimeDbContextFactory<DemoCatalogDbContext>
    {
        public DemoCatalogDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DemoCatalogDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new DemoCatalogDbContext(builder.Options);
        }
    }
}
