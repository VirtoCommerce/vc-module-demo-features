using System.Linq;
using VirtoCommerce.CatalogModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog
{
    public class DemoProduct: CatalogProduct
    {
        public DemoProductPart[] ProductParts { get; set; }

        public override object GetCopy()
        {
            var result = (DemoProduct)base.GetCopy();
            result.ProductParts = ProductParts.Select(x => x.GetCopy()).Cast<DemoProductPart>().ToArray();
            return result;
        }
    }
}
