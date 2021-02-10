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


            modelBuilder.Entity<TaggedMemberEntity>().ToTable("DemoTaggedMember").HasKey(x => x.Id);
            modelBuilder.Entity<TaggedMemberEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();

            modelBuilder.Entity<MemberEntity>().HasOne<TaggedMemberEntity>().WithOne(x => x.Member)
                .HasForeignKey<TaggedMemberEntity>(x => x.MemberId);
            //modelBuilder.Entity<TaggedMemberEntity>().HasIndex(x => x.MemberId)
            //    .IsUnique(true)
            //    .HasName("IX_MemberId");

            modelBuilder.Entity<MemberTagEntity>().ToTable("DemoMemberTag").HasKey(x => x.Id);
            modelBuilder.Entity<MemberTagEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<MemberTagEntity>().HasOne(m => m.TaggedMember).WithMany(x => x.Tags).HasForeignKey(x => x.TaggedMemberId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
