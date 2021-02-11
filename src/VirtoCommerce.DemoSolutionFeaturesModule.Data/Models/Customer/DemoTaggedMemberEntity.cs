using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer
{
    public class DemoTaggedMemberEntity : AuditableEntity
    {
        public DemoTaggedMemberEntity()
        {
            Tags = new NullCollection<DemoMemberTagEntity>();
        }

        [Required]
        [StringLength(128)]
        public string MemberId { get; set; }

        #region Navigation Properties

        public MemberEntity Member { get; set; }

        public ObservableCollection<DemoMemberTagEntity> Tags { get; set; }

        #endregion

        public virtual DemoTaggedMember ToModel(DemoTaggedMember taggedMember)
        {
            if (taggedMember == null)
            {
                throw new ArgumentNullException(nameof(taggedMember));
            }

            taggedMember.Id = Id;

            taggedMember.CreatedBy = CreatedBy;
            taggedMember.CreatedDate = CreatedDate;
            taggedMember.ModifiedBy = ModifiedBy;
            taggedMember.ModifiedDate = ModifiedDate;

            taggedMember.MemberId = MemberId;

            if (!taggedMember.Tags.IsNullCollection())
            {
                taggedMember.Tags = Tags.Select(x => x.Tag).ToList();
            }
            
            return taggedMember;
        }

        public virtual DemoTaggedMemberEntity FromModel(DemoTaggedMember taggedMember, PrimaryKeyResolvingMap pkMap)
        {
            if (taggedMember == null)
            {
                throw new ArgumentNullException(nameof(taggedMember));
            }

            if (pkMap == null)
            {
                throw new ArgumentNullException(nameof(pkMap));
            }

            pkMap.AddPair(taggedMember, this);

            Id = taggedMember.Id;

            CreatedBy = taggedMember.CreatedBy;
            CreatedDate = taggedMember.CreatedDate;
            ModifiedBy = taggedMember.ModifiedBy;
            ModifiedDate = taggedMember.ModifiedDate;

            MemberId = taggedMember.MemberId;

            if (taggedMember.Tags != null)
            {
                Tags = new ObservableCollection<DemoMemberTagEntity>(taggedMember.Tags.Select(x => new DemoMemberTagEntity()
                {
                    Tag = x
                }));
            }

            return this;
        }

        public virtual void Patch(DemoTaggedMemberEntity taggedMemberEntity)
        {
            if (taggedMemberEntity == null)
            {
                throw new ArgumentNullException(nameof(taggedMemberEntity));
            }

            taggedMemberEntity.MemberId = MemberId;

            if (!Tags.IsNullCollection())
            {
                var tagComparer = AnonymousComparer.Create((DemoMemberTagEntity x) => x.Tag);
                Tags.Patch(taggedMemberEntity.Tags, tagComparer, (sourceTag, targetTag) => targetTag.Tag = sourceTag.Tag);
            }
        }





    }
}
