using System.ComponentModel.DataAnnotations;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoOrderLineItemEntity: LineItemEntity
    {
        [StringLength(128)]
        public string ConfiguredGroupId { get; set; }

        public DemoOrderConfiguredGroupEntity ConfiguredGroup { get; set; }

        public override LineItem ToModel(LineItem lineItem)
        {
            base.ToModel(lineItem);

            if (lineItem is DemoOrderLineItem demoOrderLineItem)
            {
                demoOrderLineItem.ConfiguredGroupId = ConfiguredGroupId;
            }

            return lineItem;
        }

        public override LineItemEntity FromModel(LineItem lineItem, PrimaryKeyResolvingMap pkMap)
        {
            if (lineItem is DemoOrderLineItem demoOrderLineItem)
            {
                ConfiguredGroupId = demoOrderLineItem.ConfiguredGroupId;
            }

            return base.FromModel(lineItem, pkMap);
        }

        public override void Patch(LineItemEntity target)
        {
            base.Patch(target);

            if (target is DemoOrderLineItemEntity demoOrderLineItem)
            {
                demoOrderLineItem.ConfiguredGroupId = ConfiguredGroupId;
            }
        }
    }
}
