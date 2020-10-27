using Microsoft.EntityFrameworkCore;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;
using VirtoCommerce.OrdersModule.Data.Model;
using VirtoCommerce.OrdersModule.Data.Repositories;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoOrderDbContext: OrderDbContext
    {
        public DemoOrderDbContext(DbContextOptions<DemoOrderDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerOrderEntity>()
                .HasDiscriminator()
                .HasValue<DemoCustomerOrderEntity>(nameof(DemoCustomerOrderEntity));

            modelBuilder.Entity<LineItemEntity>()
                .HasDiscriminator()
                .HasValue<DemoOrderLineItemEntity>(nameof(DemoOrderLineItemEntity));

            modelBuilder.Entity<DemoOrderConfiguredGroupEntity>().ToTable("DemoOrderConfiguredGroup").HasKey(x => x.Id);
            modelBuilder.Entity<DemoOrderConfiguredGroupEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DemoOrderConfiguredGroupEntity>();
            modelBuilder.Entity<DemoOrderConfiguredGroupEntity>()
                .HasOne(x => x.CustomerOrder).WithMany(x => x.ConfiguredGroups).HasForeignKey(x => x.CustomerOrderId).IsRequired();

            modelBuilder.Entity<DemoOrderLineItemEntity>().HasOne(x => x.ConfiguredGroup).WithMany(x => x.Items).HasForeignKey(x => x.ConfiguredGroupId).IsRequired(false).OnDelete(DeleteBehavior.ClientCascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
