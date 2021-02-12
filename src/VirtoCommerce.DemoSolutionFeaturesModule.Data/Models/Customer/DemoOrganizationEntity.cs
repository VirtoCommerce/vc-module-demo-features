using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer
{
    public class DemoOrganizationEntity : OrganizationEntity
    {
        public override void Patch(MemberEntity target)
        {
            base.Patch(target);

            // Need to clean user groups for demo purposes 
            target.Groups = new NullCollection<MemberGroupEntity>();
        }
    }
}
