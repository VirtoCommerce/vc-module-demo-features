using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoOrderConfiguredGroup : AuditableEntity, ICloneable
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<string> ItemIds { get; set; }
        public ICollection<DemoOrderLineItem> Items { get; set; }
        public int Quantity { get; set; }

        #region Pricing

        public string Currency { get; set; }
        public virtual decimal ExtendedPrice { get; set; }
        public virtual decimal ExtendedPriceWithTax { get; set; }
        public virtual decimal Price { get; set; }
        public virtual decimal PriceWithTax { get; set; }
        public virtual decimal PlacedPrice { get; set; }
        public virtual decimal PlacedPriceWithTax { get; set; }

        #endregion Pricing

        #region Taxation

        public decimal TaxTotal { get; set; }

        public object Clone()
        {
            var result = MemberwiseClone() as DemoOrderConfiguredGroup;

            result.ItemIds = ItemIds?.Select(x => x.ToString()).ToList();
            result.Items = Items?.Select(x => x.Clone()).OfType<DemoOrderLineItem>().ToList();

            return result;
        }

        #endregion Taxation
    }
}
