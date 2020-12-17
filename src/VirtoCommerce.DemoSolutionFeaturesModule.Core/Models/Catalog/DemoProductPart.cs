using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog
{
    public class DemoProductPart : AuditableEntity, IHasName
    {
        public string ProductId { get; set; }

        public string Name { get; set; }

        public string ImgSrc { get; set; }
    }
}
