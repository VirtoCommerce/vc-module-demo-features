using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.CartModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoShoppingCart: ShoppingCart
    {
        public ICollection<DemoCartConfiguredGroup> ConfiguredGroups { get; set; }
    }
}
