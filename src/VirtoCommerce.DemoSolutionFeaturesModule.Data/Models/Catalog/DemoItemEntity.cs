using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog
{
    public class DemoItemEntity : ItemEntity
    {
        public virtual ObservableCollection<DemoProductPartEntity> ConfiguredProductParts { get; set; } = new NullCollection<DemoProductPartEntity>();

        public override CatalogProduct ToModel(CatalogProduct product, bool convertChildrens = true, bool convertAssociations = true)
        {
            var demoProduct = (DemoProduct) base.ToModel(product, convertChildrens, convertAssociations);
            demoProduct.ProductParts = ConfiguredProductParts.Select(x =>
                x.ToModel(AbstractTypeFactory<DemoProductPart>.TryCreateInstance())).ToArray();
            return demoProduct;
        }

        public override ItemEntity FromModel(CatalogProduct product, PrimaryKeyResolvingMap pkMap)
        {
            var demoProduct = (DemoProduct) product;
            var demoItemEntity = base.FromModel(product, pkMap);
            if (demoProduct.ProductParts != null)
            {
                ConfiguredProductParts = new ObservableCollection<DemoProductPartEntity>(demoProduct.ProductParts.Select(x =>
                    AbstractTypeFactory<DemoProductPartEntity>.TryCreateInstance().FromModel(x, pkMap)));
            }
            return demoItemEntity;
        }

        public override void Patch(ItemEntity target)
        {
            base.Patch(target);

            var demoItemEntity = (DemoItemEntity) target;
            if (!ConfiguredProductParts.IsNullCollection())
            {
                ConfiguredProductParts.Patch(demoItemEntity.ConfiguredProductParts, (sourcePart, targetPart) => sourcePart.Patch(targetPart));
            }
        }
    }
}
