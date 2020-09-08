using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VirtoCommerceDemoSolutionFeaturesModuleDbContext>
    {
        public VirtoCommerceDemoSolutionFeaturesModuleDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VirtoCommerceDemoSolutionFeaturesModuleDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new VirtoCommerceDemoSolutionFeaturesModuleDbContext(builder.Options);
        }
    }
}
