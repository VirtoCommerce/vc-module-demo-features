using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCartLineItemEntity : LineItemEntity
    {
        [StringLength(128)]
        public string ConfiguredGroupId { get; set; }

        public DemoCartConfiguredGroupEntity ConfiguredGroup { get; set; }

        public override LineItem ToModel(LineItem lineItem)
        {
            base.ToModel(lineItem);

            if (lineItem is DemoCartLineItem demoCartLineItem)
            {
                demoCartLineItem.ConfiguredGroupId = ConfiguredGroupId;
            }

            return lineItem;
        }

        public override LineItemEntity FromModel(LineItem lineItem, PrimaryKeyResolvingMap pkMap)
        {
            if (lineItem is DemoCartLineItem demoCartLineItem)
            {
                ConfiguredGroupId = demoCartLineItem.ConfiguredGroupId;
            }

            return base.FromModel(lineItem, pkMap);
        }

        public override void Patch(LineItemEntity target)
        {
            base.Patch(target);

            if (target is DemoCartLineItemEntity demoCartLineItem)
            {
                demoCartLineItem.ConfiguredGroupId = ConfiguredGroupId;
            }
        }
    }
}
