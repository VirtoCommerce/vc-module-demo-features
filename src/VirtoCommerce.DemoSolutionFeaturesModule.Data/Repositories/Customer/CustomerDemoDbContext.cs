using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class CustomerDemoDbContext : CustomerDbContext
    {
        public CustomerDemoDbContext(DbContextOptions<CustomerDemoDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactDemoEntity>();


            modelBuilder.Entity<DemoTaggedMemberEntity>().ToTable("DemoTaggedMember").HasKey(x => x.Id);
            modelBuilder.Entity<DemoTaggedMemberEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedNever();

            modelBuilder.Entity<MemberEntity>().HasOne<DemoTaggedMemberEntity>().WithOne(x => x.Member)
                .HasForeignKey<DemoTaggedMemberEntity>(x => x.Id);

            modelBuilder.Entity<DemoMemberTagEntity>().ToTable("DemoMemberTag").HasKey(x => x.Id);
            modelBuilder.Entity<DemoMemberTagEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<DemoMemberTagEntity>().HasOne(m => m.TaggedMember).WithMany(x => x.Tags).HasForeignKey(x => x.TaggedMemberId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
