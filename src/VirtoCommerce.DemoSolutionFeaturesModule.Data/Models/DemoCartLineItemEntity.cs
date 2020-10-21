using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCartLineItemEntity : LineItemEntity
    {
        public virtual ObservableCollection<DemoCartLineItemConfiguredGroupEntity> ItemGroups { get; set; } = new NullCollection<DemoCartLineItemConfiguredGroupEntity>();

        public override LineItem ToModel(LineItem lineItem)
        {            
            base.ToModel(lineItem);

            if (lineItem is DemoCartLineItem item)
            {
                item.ConfiguredProductId = ConfiguredProductId;
            }

            return lineItem;
        }

        public override LineItemEntity FromModel(LineItem lineItem, PrimaryKeyResolvingMap pkMap)
        {
            if (lineItem is DemoCartLineItem item)
            {
                ConfiguredProductId = item.ConfiguredProductId;
            }

            return base.FromModel(lineItem, pkMap);
        }

        public override void Patch(LineItemEntity target)
        {
            if (target is DemoCartLineItemEntity item)
            {
                item.ConfiguredProductId = ConfiguredProductId;
            }

            base.Patch(target);
        }
    }
}
