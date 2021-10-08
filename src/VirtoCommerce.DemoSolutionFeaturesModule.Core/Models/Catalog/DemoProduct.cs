using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog
{
    public class DemoProduct : CatalogProduct
    {
        public IList<DemoProductPart> ProductParts { get; set; }

        public override object GetCopy()
        {
            var result = base.GetCopy();

            if (result is DemoProduct demoProduct)
            {
                demoProduct.ProductParts = ProductParts?.Select(x => x.GetCopy()).OfType<DemoProductPart>().ToList();
            }

            return result;
        }

        public override void ReduceDetails(string responseGroup)
        {
            base.ReduceDetails(responseGroup);

            var productResponseGroup = EnumUtility.SafeParseFlags(responseGroup, ItemResponseGroup.ItemLarge);

            if (!productResponseGroup.HasFlag(ItemResponseGroup.ItemLarge))
            {
                ProductParts = null;
            }
        }
    }
}
