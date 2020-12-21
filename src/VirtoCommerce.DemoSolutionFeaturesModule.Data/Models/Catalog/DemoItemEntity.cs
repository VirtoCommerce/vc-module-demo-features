using System.Collections.ObjectModel;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog
{
    public class DemoItemEntity : ItemEntity
    {
        public virtual ObservableCollection<DemoProductPartEntity> ConfiguredProductParts { get; set; } = new NullCollection<DemoProductPartEntity>();

        public virtual ObservableCollection<DemoProductPartItemEntity> PartItems { get; set; } = new NullCollection<DemoProductPartItemEntity>();
    }
}
