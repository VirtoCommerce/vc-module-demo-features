using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer
{
    public class TaggedMemberEntity : AuditableEntity
    {
        public TaggedMemberEntity()
        {
            Tags = new NullCollection<MemberTagEntity>();
        }
        [Required]
        [StringLength(128)]
        public string MemberId { get; set; }

        [Required]
        [StringLength(128)]
        public string MemberType { get; set; }

        #region Navigation Properties

        public MemberEntity Member { get; set; }

        public ObservableCollection<MemberTagEntity> Tags { get; set; }

        #endregion

        public virtual TaggedMember ToModel(TaggedMember taggedMember)
        {
            if (taggedMember == null)
                throw new ArgumentNullException(nameof(taggedMember));

            taggedMember.Id = Id;

            taggedMember.CreatedBy = CreatedBy;
            taggedMember.CreatedDate = CreatedDate;
            taggedMember.ModifiedBy = ModifiedBy;
            taggedMember.ModifiedDate = ModifiedDate;

            taggedMember.MemberType = MemberType;
            taggedMember.MemberId = MemberId;
            if (!taggedMember.Tags.IsNullCollection())
            {
                taggedMember.Tags = Tags.Select(x => x.Tag).ToList();
            }
            
            return taggedMember;
        }

        public virtual TaggedMemberEntity FromModel(TaggedMember taggedMember, PrimaryKeyResolvingMap pkMap)
        {
            if (taggedMember == null)
                throw new ArgumentNullException(nameof(taggedMember));
            if (pkMap == null)
                throw new ArgumentNullException(nameof(pkMap));

            pkMap.AddPair(taggedMember, this);

            Id = taggedMember.Id;

            CreatedBy = taggedMember.CreatedBy;
            CreatedDate = taggedMember.CreatedDate;
            ModifiedBy = taggedMember.ModifiedBy;
            ModifiedDate = taggedMember.ModifiedDate;

            MemberType = taggedMember.MemberType;
            MemberId = taggedMember.MemberId;

            if (taggedMember.Tags != null)
            {
                Tags = new ObservableCollection<MemberTagEntity>(taggedMember.Tags.Select(x => new MemberTagEntity()
                {
                    Tag = x
                }));
            }


            return this;
        }

        public virtual void Patch(TaggedMemberEntity taggedMemberEntity)
        {
            if (taggedMemberEntity == null)
                throw new ArgumentNullException(nameof(taggedMemberEntity));

            taggedMemberEntity.MemberId = MemberId;
            taggedMemberEntity.MemberType = MemberType;

            if (!Tags.IsNullCollection())
            {
                var tagComparer = AnonymousComparer.Create((MemberTagEntity x) => x.Tag);
                Tags.Patch(taggedMemberEntity.Tags, tagComparer, (sourceTag, targetTag) => targetTag.Tag = sourceTag.Tag);
            }
        }





    }
}
