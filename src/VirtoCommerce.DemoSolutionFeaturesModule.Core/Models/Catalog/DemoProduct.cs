using System.Linq;
using VirtoCommerce.CatalogModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog
{
    public class DemoProduct : CatalogProduct
    {
        public DemoProductPart[] ProductParts { get; set; }

        public override object GetCopy()
        {
            var result = base.GetCopy();

            if (result is DemoProduct demoProduct)
            {
                demoProduct.ProductParts = ProductParts.Select(x => x.GetCopy()).Cast<DemoProductPart>().ToArray();
            }

            return result;
        }
    }
}
