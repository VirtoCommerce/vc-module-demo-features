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

        [StringLength(2083)]
        [Required]
        public string ImgSrc { get; set; }

        [StringLength(128)]
        public string DefaultItemId { get; set; }

        public int Priority { get; set; }

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
            part.ImgSrc = ImgSrc;
            part.Priority = Priority;
            part.DefaultItemId = DefaultItemId;

            if (!PartItems.IsNullOrEmpty())
            {
                part.ItemsIds = PartItems.Select(x => x.ItemId).ToArray();
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
            ImgSrc = part.ImgSrc;
            Priority = part.Priority;
            DefaultItemId = part.DefaultItemId;

            if (part.ItemsIds != null)
            {
                PartItems = new ObservableCollection<DemoProductPartItemEntity>(part.ItemsIds.Select(x => new DemoProductPartItemEntity { ConfiguredProductPartId = part.Id, ItemId = x }));
            }

            return this;
        }

        public virtual void Patch(DemoProductPartEntity target)
        {
            target.ConfiguredProductId = ConfiguredProductId;
            target.Name = Name;
            target.ImgSrc = ImgSrc;
            target.Priority = Priority;
            target.DefaultItemId = DefaultItemId;

            if (!PartItems.IsNullCollection())
            {
                target.PartItems = PartItems;
            }
        }
    }
}
