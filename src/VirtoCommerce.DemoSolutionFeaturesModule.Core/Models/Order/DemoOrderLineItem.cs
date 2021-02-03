using VirtoCommerce.OrdersModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoOrderLineItem : LineItem
    {
        public string ConfiguredGroupId { get; set; }

        public override object Clone()
        {
            var orderLineItem = base.Clone();

            if (orderLineItem is DemoOrderLineItem demoOrderLineItem)
            {
                demoOrderLineItem.ConfiguredGroupId = ConfiguredGroupId;
            }

            return orderLineItem;
        }
    }
}
