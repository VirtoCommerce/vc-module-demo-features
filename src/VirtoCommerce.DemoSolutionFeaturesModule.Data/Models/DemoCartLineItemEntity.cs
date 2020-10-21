using System.Collections.ObjectModel;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCartLineItemEntity : LineItemEntity
    {
        public virtual ObservableCollection<DemoCartLineItemConfiguredGroupEntity> ItemGroups { get; set; } = new NullCollection<DemoCartLineItemConfiguredGroupEntity>();

        public override void Patch(LineItemEntity target)
        {
            base.Patch(target);

            if (target is DemoCartLineItemEntity item)
            {
                if (!ItemGroups.IsNullCollection())
                {
                    var itemGroupsComparer = AnonymousComparer.Create((DemoCartLineItemConfiguredGroupEntity x) => new { x.ItemId, x.GroupId });
                    ItemGroups.Patch(item.ItemGroups, itemGroupsComparer, (s, t) => { });
                }
            }
        }
    }
}
