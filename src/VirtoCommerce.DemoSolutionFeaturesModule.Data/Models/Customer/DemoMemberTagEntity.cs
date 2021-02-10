using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer
{
    public class DemoMemberTagEntity: Entity
    {
        [Required]
        [StringLength(128)]
        public string Tag { get; set; }

        #region Navigation Properties
        [Required]
        [StringLength(128)]
        public string TaggedMemberId { get; set; }

        public virtual DemoTaggedMemberEntity TaggedMember { get; set; }

        #endregion
    }
}
