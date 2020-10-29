using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoOrderConfiguredGroup: AuditableEntity, ICloneable
    {
        public string ProductId { get; set; }
        public ICollection<string> ItemIds { get; set; } = new List<string>();
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
            var result = MemberwiseClone() as DemoCartConfiguredGroup;

            result.ItemIds = ItemIds?.Select(x => x.ToString()).ToList();

            return result;
        }

        #endregion Taxation
    }
}
