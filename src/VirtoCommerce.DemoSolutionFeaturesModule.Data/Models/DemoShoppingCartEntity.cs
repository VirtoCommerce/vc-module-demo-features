using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoShoppingCartEntity : ShoppingCartEntity
    {
        public virtual ObservableCollection<DemoCartConfiguredGroupEntity> ConfiguredGroups { get; set; } = new NullCollection<DemoCartConfiguredGroupEntity>();

        public override ShoppingCart ToModel(ShoppingCart cart)
        {
            var cartExtended = (DemoShoppingCart)base.ToModel(cart);

            cartExtended.ConfiguredGroups =
                ConfiguredGroups
                    .Select(x => x.ToModel(AbstractTypeFactory<DemoCartConfiguredGroup>.TryCreateInstance()))
                    .ToList();

            return cartExtended;
        }

        public override ShoppingCartEntity FromModel(ShoppingCart cart, PrimaryKeyResolvingMap pkMap)
        {           
            var cartExtended = (DemoShoppingCart)cart;

            if (cartExtended.ConfiguredGroups != null)
            {
                ConfiguredGroups = new ObservableCollection<DemoCartConfiguredGroupEntity>(
                   cartExtended.ConfiguredGroups.Select(x => AbstractTypeFactory<DemoCartConfiguredGroupEntity>.TryCreateInstance().FromModel(x, pkMap)));
            }

            return base.FromModel(cart, pkMap);
        }

        public override void Patch(ShoppingCartEntity target)
        {
            base.Patch(target);

            var targetCart = (DemoShoppingCartEntity)target;

            if (!ConfiguredGroups.IsNullCollection())
            {
                ConfiguredGroups.Patch(targetCart.ConfiguredGroups, (s, t) => s.Patch(t));
            }
        }
    }
}
