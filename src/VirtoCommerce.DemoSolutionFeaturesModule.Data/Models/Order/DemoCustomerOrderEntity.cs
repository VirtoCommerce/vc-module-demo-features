using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCustomerOrderEntity : CustomerOrderEntity
    {
        public virtual ObservableCollection<DemoOrderConfiguredGroupEntity> ConfiguredGroups { get; set; } = new NullCollection<DemoOrderConfiguredGroupEntity>();

        public override OrderOperation ToModel(OrderOperation operation)
        {
            var orderExtended = (DemoCustomerOrder)base.ToModel(operation);

            orderExtended.ConfiguredGroups =
                ConfiguredGroups
                    .Select(x => x.ToModel(AbstractTypeFactory<DemoOrderConfiguredGroup>.TryCreateInstance()))
                    .ToList();

            return orderExtended;
        }

        public override OperationEntity FromModel(OrderOperation operation, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(operation, pkMap);

            var orderExtended = (DemoCustomerOrder)operation;

            if (orderExtended.ConfiguredGroups != null)
            {
                ConfiguredGroups = new ObservableCollection<DemoOrderConfiguredGroupEntity>(
                    orderExtended.ConfiguredGroups.Select(x => AbstractTypeFactory<DemoOrderConfiguredGroupEntity>.TryCreateInstance().FromModel(x, pkMap)));
            }

            return this;
        }

        public override void Patch(OperationEntity operation)
        {
            base.Patch(operation);

            var orderdExtended = (DemoCustomerOrderEntity)operation;

            if (!ConfiguredGroups.IsNullCollection())
            {
                ConfiguredGroups.Patch(orderdExtended.ConfiguredGroups, (s, t) => s.Patch(t));
            }
        }
    }
}
