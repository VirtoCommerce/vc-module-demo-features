using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoCartDbContext : CartDbContext
    {
        public DemoCartDbContext(DbContextOptions<DemoCartDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCartEntity>().Property("Discriminator").HasMaxLength(128);
            modelBuilder.Entity<ShoppingCartEntity>()
               .HasDiscriminator()
               .HasValue<DemoShoppingCartEntity>(nameof(DemoShoppingCartEntity));

            modelBuilder.Entity<LineItemEntity>().Property("Discriminator").HasMaxLength(128);
            modelBuilder.Entity<LineItemEntity>()
                .HasDiscriminator()
                .HasValue<DemoCartLineItemEntity>(nameof(DemoCartLineItemEntity));

            modelBuilder.Entity<DemoCartConfiguredGroupEntity>().ToTable("DemoCartConfiguredGroup").HasKey(x => x.Id);
            modelBuilder.Entity<DemoCartConfiguredGroupEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DemoCartConfiguredGroupEntity>();
            modelBuilder.Entity<DemoCartConfiguredGroupEntity>()
                .HasOne(x => x.ShoppingCart).WithMany(x => x.ConfiguredGroups).HasForeignKey(x => x.ShoppingCartId).IsRequired();

            modelBuilder.Entity<DemoCartLineItemEntity>().HasOne(x => x.ConfiguredGroup).WithMany(x => x.Items).HasForeignKey(x => x.ConfiguredGroupId).IsRequired(false).OnDelete(DeleteBehavior.ClientCascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
