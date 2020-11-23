using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoOrderConfiguredGroupEntity: AuditableEntity
    {
        public string ProductId { get; set; }
        public string CustomerOrderId { get; set; }
        public virtual DemoCustomerOrderEntity CustomerOrder { get; set; }
        public virtual ObservableCollection<DemoOrderLineItemEntity> Items { get; set; } = new NullCollection<DemoOrderLineItemEntity>();
        public int Quantity { get; set; }

        #region Pricing

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }

        [Column(TypeName = "Money")]
        public decimal Price { get; set; }

        [Column(TypeName = "Money")]
        public decimal PriceWithTax { get; set; }

        #endregion Pricing

        public virtual DemoOrderConfiguredGroup ToModel(DemoOrderConfiguredGroup group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            group.Id = Id;
            group.ProductId = ProductId;
            group.CreatedDate = CreatedDate;
            group.CreatedBy = CreatedBy;
            group.ModifiedDate = ModifiedDate;
            group.ModifiedBy = ModifiedBy;
            group.Quantity = Quantity;
            group.Currency = Currency;
            group.Price = Price;
            group.PriceWithTax = PriceWithTax;

            group.ItemIds = Items.Select(x => x.Id).ToList();

            return group;
        }

        public virtual DemoOrderConfiguredGroupEntity FromModel(DemoOrderConfiguredGroup group, PrimaryKeyResolvingMap pkMap)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            pkMap.AddPair(group, this);

            Id = group.Id;
            ProductId = group.ProductId;
            CreatedDate = group.CreatedDate;
            CreatedBy = group.CreatedBy;
            ModifiedDate = group.ModifiedDate;
            ModifiedBy = group.ModifiedBy;
            Quantity = group.Quantity;
            Currency = group.Currency;
            Price = group.Price;
            PriceWithTax = group.PriceWithTax;

            return this;
        }

        public virtual void Patch(DemoOrderConfiguredGroupEntity target)
        {
            target.ProductId = ProductId;
            target.Quantity = Quantity;
            target.Currency = Currency;
            target.Price = Price;
            target.PriceWithTax = PriceWithTax;

            if (!Items.IsNullCollection())
            {
                Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
            }
        }
    }
}
