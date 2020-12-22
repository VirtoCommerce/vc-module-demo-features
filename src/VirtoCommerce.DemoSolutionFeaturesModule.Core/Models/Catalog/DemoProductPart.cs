using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog
{
    public class DemoProductPart : AuditableEntity, IHasName
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

        public string[] ItemsIds { get; set; }
    }
}
