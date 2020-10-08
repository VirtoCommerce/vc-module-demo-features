using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoShoppingCartEntity : ShoppingCartEntity
    {
        //public virtual ObservableCollection<CartLineItemGroupDemoEntity> Groups { get; set; } = new NullCollection<CartLineItemGroupDemoEntity>();

        public override ShoppingCart ToModel(ShoppingCart cart)
        {
            base.ToModel(cart);
            //if (cart is DemoShoppingCart cartDemo)
            //{
            //    cartDemo.ConfiguredItems =
            //    Groups
            //        .Select(x => x.ToModel(AbstractTypeFactory<DemoCartLineItemGroup>.TryCreateInstance()))
            //        .ToList();
            //}


            return cart;
        }

        public override ShoppingCartEntity FromModel(ShoppingCart cart, PrimaryKeyResolvingMap pkMap)
        {
            base.FromModel(cart, pkMap);

            //if (cart is DemoShoppingCart cartDemo)
            //{
            //    var groups = (cartDemo.ConfiguredItems ?? Enumerable.Empty<DemoCartLineItemGroup>())
            //        .Select(group => AbstractTypeFactory<CartLineItemGroupDemoEntity>.TryCreateInstance().FromModel(group, pkMap));

            //    Groups = new ObservableCollection<CartLineItemGroupDemoEntity>(groups);
            //}
            return this;
        }

        public override void Patch(ShoppingCartEntity target)
        {
            base.Patch(target);

            //if (target is DemoShoppingCartEntity targetCart)
            //{
            //    if (!Groups.IsNullCollection())
            //    {
            //        Groups.Patch(targetCart.Groups, (s, t) => s.Patch(t));
            //    }
            //}                
        }

    }
}
