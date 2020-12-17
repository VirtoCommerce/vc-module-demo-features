using System;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog
{
    public class DemoProductPartEntity : AuditableEntity
    {
        public string ProductId { get; set; }

        public string Name { get; set; }

        public string ImgSrc { get; set; }

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

            part.ProductId = ProductId;
            part.Name = Name;
            part.ImgSrc = ImgSrc;

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

            ProductId = part.ProductId;
            Name = part.Name;
            ImgSrc = part.ImgSrc;

            return this;
        }

        public virtual void Patch(DemoProductPartEntity target)
        {
            target.ProductId = ProductId;
            target.Name = Name;
            target.ImgSrc = ImgSrc;
        }
    }
}
