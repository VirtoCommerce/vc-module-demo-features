using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using VirtoCommerce.CoreModule.Core.Tax;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCartConfiguredGroupEntity : AuditableEntity
    {
        public string ProductId { get; set; }
        public string ShoppingCartId { get; set; }
        public virtual DemoShoppingCartEntity ShoppingCart { get; set; }
        public virtual ObservableCollection<DemoCartLineItemConfiguredGroupEntity> ItemGroups { get; set; } = new NullCollection<DemoCartLineItemConfiguredGroupEntity>();
        public int Quantity { get; set; }

        #region Pricing

        [Required]
        [StringLength(3)]
        public string Currency { get; set; }       

        [Column(TypeName = "Money")]
        public decimal ListPrice { get; set; }

        [Column(TypeName = "Money")]
        public decimal ListPriceWithTax { get; set; }

        [Column(TypeName = "Money")]
        public decimal SalePrice { get; set; }

        [Column(TypeName = "Money")]
        public decimal SalePriceWithTax { get; set; }

        #endregion

        #region Taxation

        public virtual ObservableCollection<DemoTaxDetailEntity> TaxDetails { get; set; } = new NullCollection<DemoTaxDetailEntity>();

        #endregion

        public virtual DemoCartConfiguredGroup ToModel(DemoCartConfiguredGroup group)
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
            group.ListPrice = ListPrice;
            group.ListPriceWithTax = ListPriceWithTax;
            group.SalePrice = SalePrice;
            group.SalePriceWithTax = SalePriceWithTax;

            group.ItemIds = ItemGroups.Select(x => x.ItemId).ToList();

            group.TaxDetails =
               TaxDetails.Select(x => x.ToModel(AbstractTypeFactory<TaxDetail>.TryCreateInstance()))
                         .ToList();


            return group;
        }

        public virtual DemoCartConfiguredGroupEntity FromModel(DemoCartConfiguredGroup group, PrimaryKeyResolvingMap pkMap)
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
            ListPrice = group.ListPrice;
            ListPriceWithTax = group.ListPriceWithTax;
            SalePrice = group.SalePrice;
            SalePriceWithTax = group.SalePriceWithTax;

            if (group.ItemIds != null)
            {
                ItemGroups =
                    new ObservableCollection<DemoCartLineItemConfiguredGroupEntity>(
                        group.ItemIds.Select(x => new DemoCartLineItemConfiguredGroupEntity { GroupId = Id, ItemId = x }));
            }

            if (group.TaxDetails != null)
            {
                TaxDetails = new ObservableCollection<DemoTaxDetailEntity>(
                    group.TaxDetails.Select(x =>
                                 AbstractTypeFactory<DemoTaxDetailEntity>.TryCreateInstance().FromModel(x))
                             .Cast<DemoTaxDetailEntity>());
            }

            return this;
        }

        public virtual void Patch(DemoCartConfiguredGroupEntity target)
        {
            target.ProductId = ProductId;
            target.Quantity = Quantity;
            target.Currency = Currency;
            target.ListPrice = ListPrice;
            target.ListPriceWithTax = ListPriceWithTax;
            target.SalePrice = SalePrice;
            target.SalePriceWithTax = SalePriceWithTax;

            if (!ItemGroups.IsNullCollection())
            {
                var itemGroupsComparer =
                    AnonymousComparer.Create((DemoCartLineItemConfiguredGroupEntity x) => new { x.ItemId, x.GroupId });

                ItemGroups.Patch(target.ItemGroups, itemGroupsComparer, (s, t) => { });
            }

            if (!TaxDetails.IsNullCollection())
            {
                var taxDetailComparer = AnonymousComparer.Create((DemoTaxDetailEntity x) => x.Name);
                TaxDetails.Patch(target.TaxDetails, taxDetailComparer, (s, t) => s.Patch(t));
            }
        }

    }
}
