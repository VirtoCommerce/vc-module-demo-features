using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DesignTimeCartDbContextFactory : IDesignTimeDbContextFactory<DemoCartDbContext>
    {
        public DemoCartDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DemoCartDbContext>();

            builder.UseSqlServer("Data Source=(local);Initial Cart=VirtoCommerce3;Persist Security Info=True;User ID=virto;Password=virto;MultipleActiveResultSets=True;Connect Timeout=30");

            return new DemoCartDbContext(builder.Options);
        }
    }
}
