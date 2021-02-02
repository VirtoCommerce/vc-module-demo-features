using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoCatalogDbContext : CatalogDbContext
    {
        public DemoCatalogDbContext(DbContextOptions<DemoCatalogDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DemoItemEntity>()
               .HasDiscriminator()
               .HasValue<DemoItemEntity>(nameof(DemoItemEntity));
            modelBuilder.Entity<DemoItemEntity>().Property("Discriminator").HasMaxLength(128);

            modelBuilder.Entity<DemoProductPartEntity>().ToTable("DemoProductPart").HasKey(x => x.Id);
            modelBuilder.Entity<DemoProductPartEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DemoProductPartEntity>();
            modelBuilder.Entity<DemoProductPartEntity>()
                .HasOne(x => x.ConfiguredProduct).WithMany(x => x.ConfiguredProductParts).HasForeignKey(x => x.ConfiguredProductId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            // PartItems many to many definition
            modelBuilder.Entity<DemoProductPartItemEntity>()
                .ToTable("DemoProductPartItem");
            modelBuilder.Entity<DemoProductPartItemEntity>()
                .Property(x => x.ConfiguredProductPartId).HasMaxLength(128);
            modelBuilder.Entity<DemoProductPartItemEntity>()
                .Property(x => x.ItemId).HasMaxLength(128);

            modelBuilder.Entity<DemoProductPartItemEntity>()
                .HasKey(x => new { x.ConfiguredProductPartId, x.ItemId });

            modelBuilder.Entity<DemoProductPartItemEntity>()
               .HasOne(x => x.Item)
               .WithMany()
               .HasForeignKey(x => x.ItemId)
               .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<DemoProductPartEntity>()
                .HasMany(x => x.PartItems)
                .WithOne(x => x.ConfiguredProductPart)
                .HasForeignKey(x => x.ConfiguredProductPartId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
