using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DesignTimeOrderDbContextFactory: IDesignTimeDbContextFactory<DemoOrderDbContext>
    {
        public DemoOrderDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DemoOrderDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Catalog=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new DemoOrderDbContext(builder.Options);
        }
    }
}
