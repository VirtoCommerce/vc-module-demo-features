using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories
{
    public class CartDemoDbContext : CartDbContext
    {
        public CartDemoDbContext(DbContextOptions<CartDemoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CartLineItemDemoEntity>();

            //modelBuilder.Entity<CartLineItemGroupDemoEntity>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
