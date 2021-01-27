using System;
using System.Linq;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog
{
    public class DemoProductPart : AuditableEntity, IHasName, ICloneable, ICopyable
    {
        public string ConfiguredProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsRequired { get; set; }

        public string ImgSrc { get; set; }

        public int Priority { get; set; }

        public int MinQuantity { get; set; }

        public int MaxQuantity { get; set; }

        public string DefaultItemId { get; set; }

        public ProductPartItemInfo[] PartItems { get; set; }

        public virtual object Clone()
        {
            var result = (DemoProductPart)MemberwiseClone();
            result.PartItems = PartItems?.Select(x => x.Clone()).Cast<ProductPartItemInfo>().ToArray();
            return result;
        }

        public object GetCopy()
        {
            var result = (DemoProductPart) Clone();
            result.Id = null;
            result.ConfiguredProductId = null;
            return result;
        }
    }
}
