using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class DemoCartDbContext : CartDbContext
    {
        public DemoCartDbContext(DbContextOptions<DemoCartDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineItemEntity>()
                .HasDiscriminator()
                .HasValue<DemoCartLineItemEntity>(nameof(DemoCartLineItemEntity));

            base.OnModelCreating(modelBuilder);
        }
    }
}
