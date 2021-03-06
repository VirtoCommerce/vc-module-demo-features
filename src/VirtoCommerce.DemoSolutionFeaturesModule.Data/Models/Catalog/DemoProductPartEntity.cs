using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog
{
    public class DemoProductPartEntity : AuditableEntity
    {
        public DemoItemEntity ConfiguredProduct { get; set; }

        [StringLength(128)]
        [Required]
        public string ConfiguredProductId { get; set; }

        [StringLength(1024)]
        [Required]
        public string Name { get; set; }

        [StringLength(1024)]
        public string Description { get; set; }

        public bool IsRequired { get; set; }

        [StringLength(2083)]
        public string ImgSrc { get; set; }

        public int Priority { get; set; }

        public int MinQuantity { get; set; }

        public int MaxQuantity { get; set; }

        [StringLength(128)]
        public string DefaultItemId { get; set; }

        public virtual ObservableCollection<DemoProductPartItemEntity> PartItems { get; set; } = new NullCollection<DemoProductPartItemEntity>();

        public virtual DemoProductPart ToModel(DemoProductPart part)
        {
            if (part == null)
            {
                throw new ArgumentNullException(nameof(part));
            }

            part.Id = Id;
            part.CreatedDate = CreatedDate;
            part.CreatedBy = CreatedBy;
            part.ModifiedDate = ModifiedDate;
            part.ModifiedBy = ModifiedBy;

            part.ConfiguredProductId = ConfiguredProductId;
            part.Name = Name;
            part.Description = Description;
            part.IsRequired = IsRequired;
            part.ImgSrc = ImgSrc;
            part.Priority = Priority;
            part.MinQuantity = MinQuantity;
            part.MaxQuantity = MaxQuantity;
            part.DefaultItemId = DefaultItemId;

            if (!PartItems.IsNullOrEmpty())
            {
                part.PartItems = PartItems.Select(x => new ProductPartItemInfo { ItemId = x.ItemId, Priority = x.Priority } ).ToArray();
            }

            return part;
        }

        public virtual DemoProductPartEntity FromModel(DemoProductPart part, PrimaryKeyResolvingMap pkMap)
        {
            if (part == null)
            {
                throw new ArgumentNullException(nameof(part));
            }

            pkMap.AddPair(part, this);

            Id = part.Id;
            CreatedDate = part.CreatedDate;
            CreatedBy = part.CreatedBy;
            ModifiedDate = part.ModifiedDate;
            ModifiedBy = part.ModifiedBy;

            ConfiguredProductId = part.ConfiguredProductId;
            Name = part.Name;
            Description = part.Description;
            IsRequired = part.IsRequired;
            ImgSrc = part.ImgSrc;
            Priority = part.Priority;
            MinQuantity = part.MinQuantity;
            MaxQuantity = part.MaxQuantity;
            DefaultItemId = part.DefaultItemId;

            if (part.PartItems != null)
            {
                PartItems = new ObservableCollection<DemoProductPartItemEntity>(part.PartItems.Select(x =>
                    new DemoProductPartItemEntity { ConfiguredProductPartId = part.Id, ItemId = x.ItemId, Priority = x.Priority }
                ));
            }

            return this;
        }

        public virtual void Patch(DemoProductPartEntity target)
        {
            target.ConfiguredProductId = ConfiguredProductId;
            target.Name = Name;
            target.Description = Description;
            target.IsRequired = IsRequired;
            target.ImgSrc = ImgSrc;
            target.Priority = Priority;
            target.MinQuantity = MinQuantity;
            target.MaxQuantity = MaxQuantity;
            target.DefaultItemId = DefaultItemId;

            if (!PartItems.IsNullCollection())
            {
                target.PartItems = PartItems;
            }
        }
    }
}
