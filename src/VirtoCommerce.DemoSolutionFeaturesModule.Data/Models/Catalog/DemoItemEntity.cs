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
            var result = base.ToModel(product, convertChildrens, convertAssociations);

            if (result is DemoProduct demoProduct)
            {
                demoProduct.ProductParts = ConfiguredProductParts.Select(x => x.ToModel(AbstractTypeFactory<DemoProductPart>.TryCreateInstance())).ToList();
            }

            return result;
        }

        public override ItemEntity FromModel(CatalogProduct product, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(product, pkMap);
            if (product is DemoProduct { ProductParts: { } } demoProduct)
            {
                ConfiguredProductParts = new ObservableCollection<DemoProductPartEntity>(demoProduct.ProductParts.Select(x =>
                    AbstractTypeFactory<DemoProductPartEntity>.TryCreateInstance().FromModel(x, pkMap)));
            }
            return this;
        }

        public override void Patch(ItemEntity target)
        {
            base.Patch(target);
            
            if (target is DemoItemEntity demoItemEntity && !ConfiguredProductParts.IsNullCollection())
            {
                ConfiguredProductParts.Patch(demoItemEntity.ConfiguredProductParts, (sourcePart, targetPart) => sourcePart.Patch(targetPart));
            }
        }
    }
}
