using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCustomerOrderEntity: CustomerOrderEntity
    {
        public virtual ObservableCollection<DemoOrderConfiguredGroupEntity> ConfiguredGroups { get; set; } = new NullCollection<DemoOrderConfiguredGroupEntity>();

        public override OrderOperation ToModel(OrderOperation order)
        {
            var orderExtended = (DemoCustomerOrder)base.ToModel(order);

            orderExtended.ConfiguredGroups =
                ConfiguredGroups
                    .Select(x => x.ToModel(AbstractTypeFactory<DemoOrderConfiguredGroup>.TryCreateInstance()))
                    .ToList();

            return orderExtended;
        }

        public override OperationEntity FromModel(OrderOperation order, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(order, pkMap);

            var orderExtended = (DemoCustomerOrder)order;

            if (orderExtended.ConfiguredGroups != null)
            {
                ConfiguredGroups = new ObservableCollection<DemoOrderConfiguredGroupEntity>(
                    orderExtended.ConfiguredGroups.Select(x => AbstractTypeFactory<DemoOrderConfiguredGroupEntity>.TryCreateInstance().FromModel(x, pkMap)));
            }

            return this;
        }

        public override void Patch(OperationEntity target)
        {
            base.Patch(target);

            var targetOrder = (DemoCustomerOrderEntity)target;

            if (!ConfiguredGroups.IsNullCollection())
            {
                ConfiguredGroups.Patch(targetOrder.ConfiguredGroups, (s, t) => s.Patch(t));
            }
        }
    }
}
