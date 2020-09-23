using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class CustomerDemoDbContext : CustomerDbContext
    {
        public CustomerDemoDbContext(DbContextOptions<CustomerDemoDbContext> options)
          : base(options)
        {
        }

        protected CustomerDemoDbContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactDemoEntity>();

            base.OnModelCreating(modelBuilder);
        }
    }
}

