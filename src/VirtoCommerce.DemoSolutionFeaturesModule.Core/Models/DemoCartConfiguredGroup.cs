using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCartConfiguredGroup : AuditableEntity, ICloneable
    {
        public string ProductId { get; set; }
        public ICollection<DemoCartLineItem> Items { get; set; }
        public int Quantity { get; set; }

        #region Pricing

        public string Currency { get; set; }
        public virtual decimal ExtendedPrice { get; set; }
        public virtual decimal ExtendedPriceWithTax { get; set; }
        public virtual decimal ListPrice { get; set; }
        public virtual decimal ListPriceWithTax { get; set; }
        private decimal? _salePrice;

        public virtual decimal SalePrice
        {
            get => _salePrice ?? ListPrice;
            set => _salePrice = value;
        }

        public virtual decimal SalePriceWithTax { get; set; }
        public virtual decimal PlacedPrice { get; set; }
        public virtual decimal PlacedPriceWithTax { get; set; }

        #endregion Pricing

        #region Taxation

        public decimal TaxTotal { get; set; }

        public object Clone()
        {
            var result = MemberwiseClone() as DemoCartConfiguredGroup;

            result.Items = Items?.Select(x => x.Clone()).OfType<DemoCartLineItem>().ToList();

            return result;
        }

        #endregion Taxation
    }
}
