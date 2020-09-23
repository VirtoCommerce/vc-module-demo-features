using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class VirtoCommerceDemoSolutionFeaturesModuleDbContext : CustomerDbContext
    {
        public VirtoCommerceDemoSolutionFeaturesModuleDbContext(DbContextOptions<VirtoCommerceDemoSolutionFeaturesModuleDbContext> options)
          : base(options)
        {
        }

        protected VirtoCommerceDemoSolutionFeaturesModuleDbContext(DbContextOptions options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //        modelBuilder.Entity<MyModuleEntity>().ToTable("MyModule").HasKey(x => x.Id);
            //        modelBuilder.Entity<MyModuleEntity>().Property(x => x.Id).HasMaxLength(128);

            modelBuilder.Entity<ContactDemoEntity>();

            base.OnModelCreating(modelBuilder);
        }
    }
}

