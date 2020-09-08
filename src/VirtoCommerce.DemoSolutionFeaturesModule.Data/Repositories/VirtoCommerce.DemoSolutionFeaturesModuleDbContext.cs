using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class VirtoCommerceDemoSolutionFeaturesModuleDbContext : DbContextWithTriggers
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
            //        base.OnModelCreating(modelBuilder);
        }
    }
}

