using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class CartLineItemGroupDemoEntity : Entity
    {
        [Required]
        [StringLength(64)]
        public string ProductId { get; set; }

        public string ShoppingCartId { get; set; }

        public virtual ShoppingCartEntity ShoppingCart { get; set; }

        public int Quantity { get; set; }

        public virtual ObservableCollection<DemoCartLineItemEntity> Items { get; set; } = new NullCollection<DemoCartLineItemEntity>();

        public virtual DemoCartLineItemGroup ToModel(DemoCartLineItemGroup model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Id = Id;

            model.ProductId = ProductId;
            model.ShoppingCartId = ShoppingCartId;
            model.Quantity = Quantity;

            return model;
        }

        public virtual CartLineItemGroupDemoEntity FromModel(DemoCartLineItemGroup model, PrimaryKeyResolvingMap pkMap)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Id = model.Id;

            ProductId = model.ProductId;
            ShoppingCartId = model.ShoppingCartId;
            Quantity = model.Quantity;

            return this;
        }

        public virtual void Patch(CartLineItemGroupDemoEntity target)
        {
            target.ProductId = ProductId;
            target.ShoppingCartId = ShoppingCartId;
            target.Quantity = Quantity;

        }

    }
}
