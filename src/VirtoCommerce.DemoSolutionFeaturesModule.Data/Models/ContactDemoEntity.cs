using System.ComponentModel.DataAnnotations;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class ContactDemoEntity : ContactEntity
    {
        [StringLength(128)]
        public string Title { get; set; }


        public override Member ToModel(Member member)
        {
            base.ToModel(member);

            if (member is ContactDemo contact)
            {
                contact.Title = Title;
            }

            return member;
        }

        public override MemberEntity FromModel(Member member, PrimaryKeyResolvingMap pkMap)
        {
            if (member is ContactDemo contact)
            {
                Title = contact.Title;
            }

            return base.FromModel(member, pkMap);
        }


        public override void Patch(MemberEntity target)
        {
            if (target is ContactDemoEntity contact)
            {
                contact.Title = Title;
            }

            base.Patch(target);
        }
    }
}
