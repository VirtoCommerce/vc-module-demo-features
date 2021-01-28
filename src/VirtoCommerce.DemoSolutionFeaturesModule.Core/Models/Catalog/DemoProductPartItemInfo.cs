
using System;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog
{
    public class ProductPartItemInfo : ICloneable
    {
        public string ItemId { get; set; }

        public int Priority { get; set; }

        public object Clone()
        {
            return (ProductPartItemInfo) MemberwiseClone();
        }
    }
}
