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
            modelBuilder.Entity<DemoCartLineItemConfiguredGroupEntity>().ToTable("DemoCartLineItemConfiguredGroupEntity").HasKey(x => x.Id);
            modelBuilder.Entity<DemoCartLineItemConfiguredGroupEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

            modelBuilder.Entity<LineItemEntity>()
                .HasDiscriminator()
                .HasValue<DemoCartLineItemEntity>(nameof(DemoCartLineItemEntity));

            modelBuilder.Entity<DemoCartLineItemEntity>()
                .HasMany(x => x.ItemGroups).WithOne(x => x.Item).IsRequired().HasForeignKey(x => x.ItemId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DemoCartConfiguredGroupEntity>().ToTable("DemoCartConfiguredGroupEntity").HasKey(x => x.Id);
            modelBuilder.Entity<DemoCartConfiguredGroupEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DemoCartConfiguredGroupEntity>();
            modelBuilder.Entity<DemoCartConfiguredGroupEntity>()
                .HasOne(x => x.ShoppingCart).WithMany(x => x.ConfiguredGroups).HasForeignKey(x => x.ShoppingCartId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DemoCartConfiguredGroupEntity>()
                .HasMany(x => x.ItemGroups).WithOne(x => x.Group).HasForeignKey(x => x.GroupId).IsRequired().OnDelete(DeleteBehavior.Cascade);            

            base.OnModelCreating(modelBuilder);
        }
    }
}
