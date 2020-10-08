using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.CatalogModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoConfiguredProduct : CatalogProduct
    {
        public DemoConfiguredProduct()
        {
            ProductType = GetType().Name;
        }

        public ICollection<CatalogProduct> Parts { get; set; }

    }
}
